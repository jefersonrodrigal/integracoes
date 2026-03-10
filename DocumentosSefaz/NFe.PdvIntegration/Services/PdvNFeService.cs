using NFe.Builders;
using NFe.PdvIntegration.Configuration;
using NFe.PdvIntegration.Contracts;
using NFe.PdvIntegration.Interfaces;
using NFe.Signing.Interfaces;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace NFe.PdvIntegration.Services;

public sealed class PdvNFeService : IPdvNFeService
{
    private readonly PdvIntegrationOptions _options;
    private readonly IXmlSignatureService _xmlSignatureService;
    private readonly ICertificadoProvider? _certificadoProvider;
    private readonly IPdvSefazTransmissor? _sefazTransmissor;

    public PdvNFeService(
        PdvIntegrationOptions options,
        IXmlSignatureService xmlSignatureService,
        ICertificadoProvider? certificadoProvider = null,
        IPdvSefazTransmissor? sefazTransmissor = null)
    {
        _options = options;
        _xmlSignatureService = xmlSignatureService;
        _certificadoProvider = certificadoProvider;
        _sefazTransmissor = sefazTransmissor;
    }

    public async Task<PdvEmissaoResponse> EmitirAsync(PdvEmissaoRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.DocumentoZeus);

        if (string.IsNullOrWhiteSpace(request.CorrelationId))
        {
            request.CorrelationId = Guid.NewGuid().ToString("N");
        }

        var avisos = new List<string>();
        var assinar = request.AssinarXml ?? _options.AssinarXmlPorPadrao;
        var enviar = request.EnviarParaSefaz ?? _options.EnviarParaSefazPorPadrao;

        var xmlGerado = request.Modelo == DocumentoFiscalModelo.NFCe
            ? new NFCeBuilder().GerarXml(request.DocumentoZeus)
            : new NFeBuilder().GerarXml(request.DocumentoZeus);

        string? xmlAssinado = null;
        if (assinar)
        {
            var certificado = ObterCertificado(request.EmpresaId);
            xmlAssinado = AssinarXml(xmlGerado, certificado);
        }
        else
        {
            avisos.Add("XML gerado sem assinatura digital.");
        }

        PdvSefazRetorno? retornoSefaz = null;
        if (enviar)
        {
            if (_sefazTransmissor is null)
            {
                throw new InvalidOperationException("Transmissão para SEFAZ solicitada, mas nenhum IPdvSefazTransmissor foi configurado.");
            }

            var xmlParaEnvio = xmlAssinado ?? xmlGerado;
            retornoSefaz = await _sefazTransmissor.EnviarAsync(xmlParaEnvio, request.Modelo, cancellationToken);
        }
        else
        {
            avisos.Add("Transmissão para SEFAZ não executada.");
        }

        return new PdvEmissaoResponse
        {
            Sucesso = retornoSefaz?.Sucesso ?? true,
            CorrelationId = request.CorrelationId,
            Modelo = request.Modelo,
            XmlGerado = xmlGerado,
            XmlAssinado = xmlAssinado,
            RetornoSefaz = retornoSefaz,
            ProcessadoEm = DateTimeOffset.UtcNow,
            Avisos = avisos
        };
    }

    private X509Certificate2 ObterCertificado(string? empresaId)
    {
        var certificado = _certificadoProvider?.ObterCertificado(empresaId);

        if (certificado is null && _options.ExigirCertificadoParaAssinatura)
        {
            throw new InvalidOperationException("Assinatura habilitada, mas nenhum certificado foi disponibilizado por ICertificadoProvider.");
        }

        return certificado ?? throw new InvalidOperationException("Certificado digital não encontrado.");
    }

    private string AssinarXml(string xmlGerado, X509Certificate2 certificado)
    {
        var xmlDoc = new XmlDocument { PreserveWhitespace = true };
        xmlDoc.LoadXml(xmlGerado);

        var assinado = _xmlSignatureService.Sign(xmlDoc, certificado);
        return assinado.OuterXml;
    }
}
