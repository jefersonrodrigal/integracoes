using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Xml;

namespace NFe.Signing;

public class XmlSigner
{
    public XmlDocument Sign(XmlDocument xmlDoc, X509Certificate2 certificate)
    {
        if (!certificate.HasPrivateKey)
            throw new InvalidOperationException("Certificado não possui chave privada.");

        xmlDoc.PreserveWhitespace = true;

        var signedXml = new SignedXml(xmlDoc)
        {
            SigningKey = certificate.GetRSAPrivateKey()
        };

        var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

        var infNFeNode = xmlDoc.SelectSingleNode("//nfe:infNFe", nsmgr)
            ?? throw new InvalidOperationException("Elemento infNFe não encontrado.");

        var id = infNFeNode.Attributes["Id"]?.Value
            ?? throw new InvalidOperationException("Atributo Id não encontrado.");

        var reference = new Reference
        {
            Uri = "#" + id,
            DigestMethod = SignedXml.XmlDsigSHA256Url
        };

        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        reference.AddTransform(new XmlDsigC14NTransform());

        signedXml.AddReference(reference);

        signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
        signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl;

        var keyInfo = new KeyInfo();
        keyInfo.AddClause(new KeyInfoX509Data(certificate));
        signedXml.KeyInfo = keyInfo;

        signedXml.ComputeSignature();

        var signatureElement = signedXml.GetXml();
        infNFeNode.AppendChild(xmlDoc.ImportNode(signatureElement, true));

        return xmlDoc;
    }

    private string GetInfNFeId(XmlDocument doc)
    {
        var node = doc.GetElementsByTagName("infNFe")[0];
        return node.Attributes["Id"].Value;
    }
}
