using System.Xml.Serialization;

namespace NFe.Transmission.Models;

[XmlRoot("consReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
public class ConsReciNFe
{
    [XmlAttribute("versao")]
    public string Versao { get; set; } = "4.00";

    public string tpAmb { get; set; }
    public string nRec { get; set; }
}
