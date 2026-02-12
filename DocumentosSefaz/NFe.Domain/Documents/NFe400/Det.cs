using NFe.Domain.Documents.NFe400;
using System.Xml.Serialization;

namespace NFe.Domain.Models;

public class Det
{
    [XmlAttribute("nItem")]
    public int nItem { get; set; }

    public Prod prod { get; set; }
    public Imposto imposto { get; set; }
}
