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
        xmlDoc.PreserveWhitespace = true;

        var signedXml = new SignedXml(xmlDoc);

        signedXml.SigningKey = certificate.GetRSAPrivateKey();

        // Referência ao Id da infNFe
        var reference = new Reference();
        reference.Uri = "#" + GetInfNFeId(xmlDoc);

        // Transform enveloped
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());

        // Canonicalização
        reference.AddTransform(new XmlDsigC14NTransform());

        reference.DigestMethod = SignedXml.XmlDsigSHA1Url;

        signedXml.AddReference(reference);

        signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
        signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl;

        // Certificado
        var keyInfo = new KeyInfo();
        keyInfo.AddClause(new KeyInfoX509Data(certificate));
        signedXml.KeyInfo = keyInfo;

        signedXml.ComputeSignature();

        XmlElement signatureElement = signedXml.GetXml();

        // Inserir dentro do infNFe
        XmlNode infNFeNode = xmlDoc.GetElementsByTagName("infNFe")[0];
        infNFeNode.AppendChild(xmlDoc.ImportNode(signatureElement, true));

        return xmlDoc;
    }

    private string GetInfNFeId(XmlDocument doc)
    {
        var node = doc.GetElementsByTagName("infNFe")[0];
        return node.Attributes["Id"].Value;
    }
}
