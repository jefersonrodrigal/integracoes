using System.Collections.Generic;
using System.Xml.Serialization;
using NFe.Domain.Documents.NFe400;

namespace NFe.Transmission.Models;

[XmlRoot("enviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
public class EnviNFe
{
    [XmlAttribute("versao")]
    public string Versao { get; set; } = "4.00";

    public string idLote { get; set; }

    public int indSinc { get; set; } // 0 = assíncrono, 1 = síncrono

    [XmlElement("NFe")]
    public List<NFe> NFe { get; set; }
}
