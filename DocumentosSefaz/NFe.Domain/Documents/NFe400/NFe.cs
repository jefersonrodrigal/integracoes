using NFe.Domain.Models;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400;

[XmlRoot("NFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
public class NFe
{
    [XmlElement("infNFe")]
    public InfNFe InfNFe { get; set; }

    [XmlElement("infNFeSupl")]
    public InfNFeSupl InfNFeSupl { get; set; } // NFCe QRCode
}
