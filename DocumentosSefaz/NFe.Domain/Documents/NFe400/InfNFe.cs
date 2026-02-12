using NFe.Domain.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400;

public class InfNFe
{
    [XmlAttribute("versao")]
    public string Versao { get; set; }

    [XmlAttribute("Id")]
    public string Id { get; set; }

    public Ide ide { get; set; }
    public Emit emit { get; set; }
    public Dest dest { get; set; }

    [XmlElement("det")]
    public List<Det> det { get; set; }

    public Total total { get; set; }
    public Transp transp { get; set; }
    public Pag pag { get; set; }
    public InfAdic infAdic { get; set; }
}
