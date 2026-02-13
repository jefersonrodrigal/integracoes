using NFe.Signing.Interfaces;
using NFe.Transmission;
using NFe.Transmission.Interfaces;
using NFe.Transmission.Response;
using NFe.Transmission.Results;
using NFe.Transmission.Services.Enuns;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using NFe.Validation;
using System.Xml.Serialization;
using NFeDocumento = NFe.Domain.Documents.NFe400.NFe;

public class NFeSender
{
    private readonly XmlSchemaValidate _validator;
    private readonly IXmlSignatureService _signer;
    private readonly NFeClient _client;
    private readonly ISefazHealthChecker _healthChecker;
    private readonly X509Certificate2 _certificado;

    public NFeSender(
        XmlSchemaValidate validator,
        IXmlSignatureService signer,
        NFeClient client,
        ISefazHealthChecker healthChecker,
        X509Certificate2 certificado)
    {
        _validator = validator;
        _signer = signer;
        _client = client;
        _healthChecker = healthChecker;
        _certificado = certificado;
    }

    public async Task<NFeSendResult> EnviarAsync(NFeDocumento notaOriginal)
    {
        try
        {
            var resultadoNormal = await EnviarFluxoNormalAsync(notaOriginal);

            if (resultadoNormal.Sucesso)
                return resultadoNormal;

            if (_healthChecker.DeveAtivarContingencia(new SefazResponse
             {
                 StatusCode = int.TryParse(resultadoNormal.Codigo, out var parsed)
                     ? parsed
                     : null
             }))
            {
                return await EnviarFluxoContingenciaAsync(notaOriginal);
            }

            return resultadoNormal;
        }
        catch
        {
            // Falha técnica → contingência
            if (_healthChecker.DeveAtivarContingencia(
                new SefazResponse { HouveFalhaComunicacao = true }))
            {
                return await EnviarFluxoContingenciaAsync(notaOriginal);
            }

            throw;
        }
    }

    // ---------------------------
    // FLUXO NORMAL
    // ---------------------------
    private async Task<NFeSendResult> EnviarFluxoNormalAsync(NFeDocumento nota)
    {
        var xml = PrepararXml(nota);
        var retornoXml = await _client.EnviarAsync(xml, UrlNormal());

        return InterpretarResultado(retornoXml, false);
    }

    // ---------------------------
    // FLUXO CONTINGÊNCIA
    // ---------------------------
    private async Task<NFeSendResult> EnviarFluxoContingenciaAsync(NFeDocumento notaOriginal)
    {
        var notaContingencia = ClonarNota(notaOriginal);

        notaContingencia.InfNFe.Ide.TpEmis = TipoEmissao.SVCRS;
        notaContingencia.InfNFe.Ide.DhCont = DateTimeOffset.Now;
        notaContingencia.InfNFe.Ide.XJust = "Contingência automática SVC";

        var xml = PrepararXml(notaContingencia);
        var retornoXml = await _client.EnviarAsync(xml, UrlSVC());

        return InterpretarResultado(retornoXml, true);
    }

    // ---------------------------
    // PREPARAÇÃO XML
    // ---------------------------
    private string PrepararXml(NFeDocumento nota)
    {
        var xml = Serialize(nota);
        _validator.ValidateOrThrow(xml);
        return Assinar(xml);
    }

    private string Assinar(string xml)
    {
        var doc = new XmlDocument();
        doc.PreserveWhitespace = true;
        doc.LoadXml(xml);

        var signed = _signer.Sign(doc, _certificado);
        return signed.OuterXml;
    }

    private string Serialize(NFeDocumento nota)
    {
        var serializer = new XmlSerializer(typeof(NFeDocumento));

        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, "http://www.portalfiscal.inf.br/nfe");

        using var sw = new StringWriter();
        serializer.Serialize(sw, nota, ns);

        return sw.ToString();
    }

    // ---------------------------
    // INTERPRETAÇÃO
    // ---------------------------
    private NFeSendResult InterpretarResultado(string retornoXml, bool contingencia)
    {
        var resultado = RejeicaoHandler.Processar(retornoXml);

        return new NFeSendResult
        {
            Sucesso = resultado.Sucesso,
            EmContingencia = contingencia,
            Codigo = resultado.Codigo,
            Mensagem = resultado.Mensagem,
            XmlAutorizado = retornoXml
        };
    }

    // ---------------------------
    // CLONE SEGURO
    // ---------------------------
    private NFeDocumento ClonarNota(NFeDocumento nota)
    {
        var serializer = new XmlSerializer(typeof(NFeDocumento));

        using var ms = new MemoryStream();
        serializer.Serialize(ms, nota);

        ms.Position = 0;
        return (NFeDocumento)serializer.Deserialize(ms)!;
    }

    private string UrlNormal()
        => "https://homologacao.nfe.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx";

    private string UrlSVC()
        => "https://www.svc.fazenda.gov.br/NFeAutorizacao4.asmx";
}
