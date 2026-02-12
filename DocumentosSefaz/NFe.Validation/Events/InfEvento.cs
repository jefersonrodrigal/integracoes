using System;
using System.Xml.Serialization;

namespace NFe.Transmission.Events;

public class InfEvento
{
    [XmlAttribute("Id")]
    public string Id { get; set; }

    public int cOrgao { get; set; }
    public int tpAmb { get; set; }
    public string CNPJ { get; set; }
    public string chNFe { get; set; }
    public DateTime dhEvento { get; set; }
    public int tpEvento { get; set; } = 110111;
    public string nSeqEvento { get; set; } = "1";
    public string verEvento { get; set; } = "1.00";

    public DetEvento detEvento { get; set; }
}
