using System.Xml.Serialization;

namespace NFe.Transmission.Events;

public class DetEvento
{
    [XmlAttribute("versao")]
    public string versao { get; set; } = "1.00";

    public string descEvento { get; set; } = "Cancelamento";
    public string nProt { get; set; }
    public string xJust { get; set; }
}
