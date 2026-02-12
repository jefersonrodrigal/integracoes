using System.Xml.Serialization;

namespace NFe.Transmission.Inutilizacao;

[XmlRoot("inutNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
public class InutNFe
{
    [XmlAttribute("versao")]
    public string versao { get; set; } = "4.00";

    public InfInut infInut { get; set; }
}
