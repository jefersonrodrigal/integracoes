using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFe.Transmission.Events;

[XmlRoot("envEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
public class EnvEvento
{
    [XmlAttribute("versao")]
    public string versao { get; set; } = "1.00";

    public string idLote { get; set; }

    [XmlElement("evento")]
    public List<Evento> evento { get; set; }
}
