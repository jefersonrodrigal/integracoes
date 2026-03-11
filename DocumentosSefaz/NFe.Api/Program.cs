using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using NFe.Api.Configuration;
using NFe.Api.Contracts;
using NFe.Api.Services;
using NFe.PdvIntegration.Contracts;
using NFe.PdvIntegration.Interfaces;
using NFe.PdvIntegration.Services;
using NFe.Signing.Interfaces;
using NFe.Signing.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.Configure<SefazTransmissionOptions>(builder.Configuration.GetSection("SefazTransmission"));

var permitirSemCertificado = builder.Configuration.GetValue<bool>("Certificado:PermitirSemCertificado");
X509Certificate2? certificado = null;

if (!permitirSemCertificado)
{
    certificado = CarregarCertificado(builder.Configuration);
    builder.Services.AddSingleton<ICertificadoProvider>(new FixedCertificadoProvider(certificado));
    builder.Services.AddHttpClient<IPdvSefazTransmissor, SefazSoapTransmissor>()
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificado);
            return handler;
        });
}
else
{
    builder.Services.AddSingleton<ICertificadoProvider, NullCertificadoProvider>();
    builder.Services.AddSingleton<IPdvSefazTransmissor, FakeSefazTransmissor>();
}

builder.Services.AddSingleton<IXmlSignatureService, XmlSignatureService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var transmissao = app.MapGroup("/api/transmissao").WithTags("Transmissao");

transmissao.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    servico = "NFe.Api.Transmissao",
    modoSemCertificado = permitirSemCertificado,
    timestamp = DateTimeOffset.UtcNow
}));

transmissao.MapPost("/enviar", async (
    [FromBody] TransmitirXmlRequest request,
    IPdvSefazTransmissor transmissor,
    CancellationToken cancellationToken) =>
{
    if (request is null)
    {
        return Results.BadRequest(CriarErro("payload_invalido", "Payload invalido.", null));
    }

    if (string.IsNullOrWhiteSpace(request.XmlAssinado))
    {
        return Results.BadRequest(CriarErro("xml_obrigatorio", "Campo 'xmlAssinado' e obrigatorio.", request.CorrelationId));
    }

    if (string.IsNullOrWhiteSpace(request.CorrelationId))
    {
        request.CorrelationId = Guid.NewGuid().ToString("N");
    }

    try
    {
        ValidarXml(request.XmlAssinado);
        var retorno = await transmissor.EnviarAsync(request.XmlAssinado, request.Modelo, cancellationToken);
        if (!retorno.Sucesso)
        {
            return Results.Json(CriarErroSefaz(request.CorrelationId, retorno), statusCode: MapSefazStatusToHttp(retorno.Codigo));
        }

        return Results.Ok(CriarResposta(request.CorrelationId, retorno));
    }
    catch (XmlException ex)
    {
        return Results.BadRequest(CriarErro("xml_invalido", "XML invalido.", request.CorrelationId, ex.Message));
    }
    catch (TaskCanceledException)
    {
        return Results.Json(CriarErro("timeout_sefaz", "Timeout ao comunicar com SEFAZ.", request.CorrelationId), statusCode: StatusCodes.Status504GatewayTimeout);
    }
    catch (HttpRequestException ex)
    {
        return Results.BadRequest(CriarErro("falha_sefaz", "Falha de comunicacao com SEFAZ.", request.CorrelationId, ex.Message));
    }
    catch (Exception ex)
    {
        return Results.Json(CriarErro("erro_interno", "Erro interno ao transmitir.", request.CorrelationId, ex.Message), statusCode: StatusCodes.Status500InternalServerError);
    }
})
.WithName("EnviarDocumentoFiscal")
.WithDescription("Recebe XML assinado de NF-e/NFC-e e transmite para a SEFAZ.")
.Produces<TransmitirXmlResponse>(StatusCodes.Status200OK)
.Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
.Produces<ApiErrorResponse>(StatusCodes.Status409Conflict)
.Produces<ApiErrorResponse>(StatusCodes.Status422UnprocessableEntity)
.Produces<ApiErrorResponse>(StatusCodes.Status502BadGateway)
.Produces<ApiErrorResponse>(StatusCodes.Status503ServiceUnavailable)
.Produces(StatusCodes.Status504GatewayTimeout)
.Produces<ApiErrorResponse>(StatusCodes.Status500InternalServerError);

transmissao.MapPost("/assinar-e-enviar", async (
    [FromBody] AssinarEEnviarXmlRequest request,
    IXmlSignatureService assinaturaService,
    ICertificadoProvider certificadoProvider,
    IPdvSefazTransmissor transmissor,
    CancellationToken cancellationToken) =>
{
    if (request is null)
    {
        return Results.BadRequest(CriarErro("payload_invalido", "Payload invalido.", null));
    }

    if (string.IsNullOrWhiteSpace(request.Xml))
    {
        return Results.BadRequest(CriarErro("xml_obrigatorio", "Campo 'xml' e obrigatorio.", request.CorrelationId));
    }

    if (string.IsNullOrWhiteSpace(request.CorrelationId))
    {
        request.CorrelationId = Guid.NewGuid().ToString("N");
    }

    try
    {
        ValidarXml(request.Xml);

        string xmlAssinado;
        if (permitirSemCertificado)
        {
            // Modo dev: nao assina, apenas reaproveita o XML informado.
            xmlAssinado = request.Xml;
        }
        else
        {
            var certificadoLocal = certificadoProvider.ObterCertificado(request.EmpresaId);
            if (certificadoLocal is null)
            {
                return Results.BadRequest(CriarErro("certificado_nao_encontrado", "Certificado nao encontrado para assinatura.", request.CorrelationId));
            }

            var xmlDoc = new XmlDocument { PreserveWhitespace = true };
            xmlDoc.LoadXml(request.Xml);
            xmlAssinado = assinaturaService.Sign(xmlDoc, certificadoLocal).OuterXml;
        }

        var retorno = await transmissor.EnviarAsync(xmlAssinado, request.Modelo, cancellationToken);

        if (!retorno.Sucesso)
        {
            return Results.Json(CriarErroSefaz(request.CorrelationId, retorno), statusCode: MapSefazStatusToHttp(retorno.Codigo));
        }

        return Results.Ok(CriarResposta(request.CorrelationId, retorno));
    }
    catch (XmlException ex)
    {
        return Results.BadRequest(CriarErro("xml_invalido", "XML invalido.", request.CorrelationId, ex.Message));
    }
    catch (CryptographicException ex)
    {
        return Results.UnprocessableEntity(CriarErro("assinatura_falhou", "Falha ao assinar o XML.", request.CorrelationId, ex.Message));
    }
    catch (InvalidOperationException ex)
    {
        return Results.UnprocessableEntity(CriarErro("assinatura_falhou", "Falha ao assinar o XML.", request.CorrelationId, ex.Message));
    }
    catch (TaskCanceledException)
    {
        return Results.Json(CriarErro("timeout_sefaz", "Timeout ao comunicar com SEFAZ.", request.CorrelationId), statusCode: StatusCodes.Status504GatewayTimeout);
    }
    catch (HttpRequestException ex)
    {
        return Results.BadRequest(CriarErro("falha_sefaz", "Falha de comunicacao com SEFAZ.", request.CorrelationId, ex.Message));
    }
    catch (Exception ex)
    {
        return Results.Json(CriarErro("erro_interno", "Erro interno ao transmitir.", request.CorrelationId, ex.Message), statusCode: StatusCodes.Status500InternalServerError);
    }
})
.WithName("AssinarEEnviarDocumentoFiscal")
.WithDescription("Recebe XML nao assinado de NF-e/NFC-e, assina com certificado A1 e transmite para a SEFAZ.")
.Produces<TransmitirXmlResponse>(StatusCodes.Status200OK)
.Produces<ApiErrorResponse>(StatusCodes.Status400BadRequest)
.Produces<ApiErrorResponse>(StatusCodes.Status409Conflict)
.Produces<ApiErrorResponse>(StatusCodes.Status422UnprocessableEntity)
.Produces<ApiErrorResponse>(StatusCodes.Status502BadGateway)
.Produces<ApiErrorResponse>(StatusCodes.Status503ServiceUnavailable)
.Produces(StatusCodes.Status504GatewayTimeout)
.Produces<ApiErrorResponse>(StatusCodes.Status500InternalServerError);

app.Run();

static void ValidarXml(string xml)
{
    var xmlDoc = new XmlDocument { PreserveWhitespace = true };
    xmlDoc.LoadXml(xml);
}

static TransmitirXmlResponse CriarResposta(string correlationId, PdvSefazRetorno retorno)
{
    return new TransmitirXmlResponse
    {
        Sucesso = retorno.Sucesso,
        CorrelationId = correlationId,
        Codigo = retorno.Codigo,
        Mensagem = retorno.Mensagem,
        EmContingencia = retorno.EmContingencia,
        XmlRetorno = retorno.XmlRetorno ?? string.Empty,
        ProcessadoEm = DateTimeOffset.UtcNow
    };
}

static ApiErrorResponse CriarErro(string codigo, string mensagem, string? correlationId, string? detalhes = null)
{
    return new ApiErrorResponse
    {
        Codigo = codigo,
        Mensagem = mensagem,
        CorrelationId = correlationId ?? string.Empty,
        Detalhes = detalhes,
        ProcessadoEm = DateTimeOffset.UtcNow
    };
}

static ApiErrorResponse CriarErroSefaz(string correlationId, PdvSefazRetorno retorno)
{
    return new ApiErrorResponse
    {
        Codigo = $"sefaz_{retorno.Codigo}",
        Mensagem = retorno.Mensagem,
        CorrelationId = correlationId,
        Detalhes = $"cStat={retorno.Codigo}",
        ProcessadoEm = DateTimeOffset.UtcNow
    };
}

static int MapSefazStatusToHttp(string? cStat)
{
    if (!int.TryParse(cStat, out var code))
    {
        return StatusCodes.Status502BadGateway;
    }

    return code switch
    {
        100 or 103 or 104 => StatusCodes.Status200OK,
        108 or 109 => StatusCodes.Status503ServiceUnavailable,
        204 => StatusCodes.Status409Conflict,
        225 => StatusCodes.Status422UnprocessableEntity,
        999 => StatusCodes.Status502BadGateway,
        _ when code >= 500 => StatusCodes.Status502BadGateway,
        _ when code >= 400 => StatusCodes.Status422UnprocessableEntity,
        _ when code >= 200 => StatusCodes.Status422UnprocessableEntity,
        _ => StatusCodes.Status400BadRequest
    };
}

static X509Certificate2 CarregarCertificado(IConfiguration configuration)
{
    var pfxPath = configuration["Certificado:PfxPath"];
    var pfxPassword = configuration["Certificado:PfxPassword"];

    if (string.IsNullOrWhiteSpace(pfxPath) || string.IsNullOrWhiteSpace(pfxPassword))
    {
        throw new InvalidOperationException("Configure Certificado:PfxPath e Certificado:PfxPassword para assinatura/transmissao em homologacao.");
    }

    if (!File.Exists(pfxPath))
    {
        throw new FileNotFoundException($"Arquivo de certificado nao encontrado: {pfxPath}");
    }

    return X509CertificateLoader.LoadPkcs12FromFile(
        pfxPath,
        pfxPassword,
        X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
}

sealed class NullCertificadoProvider : ICertificadoProvider
{
    public X509Certificate2? ObterCertificado(string? empresaId = null) => null;
}
