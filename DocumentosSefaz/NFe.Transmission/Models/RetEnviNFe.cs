using System.Xml.Serialization;

namespace NFe.Transmission.Models;

[XmlRoot("retEnviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
public class RetEnviNFe
{
    public string tpAmb { get; set; }
    public string verAplic { get; set; }
    public string cStat { get; set; }
    public string xMotivo { get; set; }
    public string cUF { get; set; }
    public string dhRecbto { get; set; }
    public string infRec { get; set; }
}
