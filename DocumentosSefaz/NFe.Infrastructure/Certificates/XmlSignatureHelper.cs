using System.Security.Cryptography.X509Certificates;
using System.Xml;
using NFe.Signing;

namespace NFe.Infrastructure.Certificates
{
    public static class XmlSignatureHelper
    {
        /// <summary>
        /// Assina um XmlDocument usando o certificado digital informado e o ID de referência.
        /// </summary>
        /// <param name="xml">XmlDocument a ser assinado</param>
        /// <param name="certificate">Certificado digital</param>
        /// <param name="referenceId">ID do elemento a ser referenciado na assinatura (ex: "NFe123..." ou "infNFe123...")</param>
        /// <returns>XmlDocument assinado</returns>
        public static XmlDocument SignXml(XmlDocument xml, X509Certificate2 certificate, string referenceId)
        {
            var signer = new NFeSigner();
            return signer.Sign(xml, certificate, referenceId);
        }
    }
}
