using NFe.Signing;
using NFe.Transmission;
using NFe.Transmission.Results;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class NFeSender
{
    private readonly XmlSchemaValidator _validator;
    private readonly XmlSigner _signer;
    private readonly NFeClient _client;
    private readonly X509Certificate2 _certificado;

    public NFeSender(
        XmlSchemaValidator validator,
        XmlSigner signer,
        NFeClient client,
        X509Certificate2 certificado)
    {
        _validator = validator;
        _signer = signer;
        _client = client;
        _certificado = certificado;
    }

    public async Task<NFeSendResult> EnviarAsync(NFe nota)
    {
        try
        {
            // 1️⃣ Serializar
            var xml = Serialize(nota);

            // 2️⃣ Validar
            _validator.ValidateOrThrow(xml);

            // 3️⃣ Assinar
            var xmlAssinado = Assinar(xml);

            // 4️⃣ Enviar normal
            var retorno = await _client.EnviarAsync(xmlAssinado, UrlNormal());

            var resultado = RejeicaoHandler.Processar(retorno);

            // 5️⃣ Se sucesso
            if (resultado.Sucesso)
            {
                return new NFeSendResult
                {
                    Sucesso = true,
                    Codigo = resultado.Codigo,
                    Mensagem = resultado.Mensagem,
                    XmlAutorizado = retorno
                };
            }

            // 6️⃣ Se deve ativar contingência
            if (SefazHealthChecker.DeveAtivarContingencia(resultado.Codigo))
            {
                return await EnviarContingencia(nota);
            }

            // 7️⃣ Erro de negócio
            return new NFeSendResult
            {
                Sucesso = false,
                Codigo = resultado.Codigo,
                Mensagem = resultado.Mensagem
            };
        }
        catch (Exception ex)
        {
            // Timeout, erro de rede, etc.
            return await EnviarContingencia(nota);
        }
    }

    private async Task<NFeSendResult> EnviarContingencia(NFe nota)
    {
        nota.InfNFe.ide.tpEmis = 7;
        nota.InfNFe.ide.dhCont = DateTime.Now;
        nota.InfNFe.ide.xJust = "Contingencia automatica SVC";

        var xml = Serialize(nota);
        var xmlAssinado = Assinar(xml);

        var retorno = await _client.EnviarAsync(xmlAssinado, UrlSVC());

        var resultado = RejeicaoHandler.Processar(retorno);

        return new NFeSendResult
        {
            Sucesso = resultado.Sucesso,
            EmContingencia = true,
            Codigo = resultado.Codigo,
            Mensagem = resultado.Mensagem,
            XmlAutorizado = retorno
        };
    }

    private string Assinar(string xml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var signed = _signer.Sign(doc, _certificado);

        return signed.OuterXml;
    }

    private string Serialize(NFe nota)
    {
        var serializer = new XmlSerializer(typeof(NFe));
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "http://www.portalfiscal.inf.br/nfe");

        using var sw = new StringWriter();
        serializer.Serialize(sw, nota, ns);

        return sw.ToString();
    }

    private string UrlNormal()
        => "https://homologacao.nfe.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx";

    private string UrlSVC()
        => "https://www.svc.fazenda.gov.br/NFeAutorizacao4.asmx";
}
