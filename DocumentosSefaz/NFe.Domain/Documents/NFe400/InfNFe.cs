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

    [XmlElement("ide")]
    public Ide Ide { get; set; }

    [XmlElement("emit")]
    public Emit Emit { get; set; }

    [XmlElement("dest")]
    public Dest Dest { get; set; }

    [XmlElement("det")]
    public List<Det> Det { get; set; } = new();

    [XmlElement("total")]
    public Total Total { get; set; }

    [XmlElement("transp")]
    public Transp Transp { get; set; }

    [XmlElement("pag")]
    public Pag Pag { get; set; }

    [XmlElement("infAdic")]
    public InfAdic InfAdic { get; set; }
}
