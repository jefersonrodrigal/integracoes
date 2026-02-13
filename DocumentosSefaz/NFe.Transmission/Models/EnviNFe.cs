using System.Collections.Generic;
using System.Xml.Serialization;
using NFeDocumento = NFe.Domain.Documents.NFe400.NFe;

namespace NFe.Transmission.Models;

[XmlRoot("enviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
public class EnviNFe
{
    [XmlAttribute("versao")]
    public string Versao { get; set; } = "4.00";

    public string idLote { get; set; }

    public int indSinc { get; set; }

    [XmlElement("NFe")]
    public List<NFeDocumento> NFe { get; set; }
}
