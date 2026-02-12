using System.Xml.Serialization;

namespace NFe.Transmission.Events;

public class Evento
{
    [XmlAttribute("versao")]
    public string versao { get; set; } = "1.00";

    public InfEvento infEvento { get; set; }
}
