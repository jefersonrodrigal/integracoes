using NFe.Domain.Models;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400;

public class ICMS
{
    [XmlElement("ICMS00", typeof(ICMS00))]
    [XmlElement("ICMS10", typeof(ICMS10))]
    [XmlElement("ICMS20", typeof(ICMS20))]
    [XmlElement("ICMS30", typeof(ICMS30))]
    [XmlElement("ICMS40", typeof(ICMS40))]
    [XmlElement("ICMS51", typeof(ICMS51))]
    [XmlElement("ICMS60", typeof(ICMS60))]
    [XmlElement("ICMS70", typeof(ICMS70))]
    [XmlElement("ICMS90", typeof(ICMS90))]
    [XmlElement("ICMSSN101", typeof(ICMSSN101))]
    [XmlElement("ICMSSN102", typeof(ICMSSN102))]
    [XmlElement("ICMSSN201", typeof(ICMSSN201))]
    [XmlElement("ICMSSN202", typeof(ICMSSN202))]
    [XmlElement("ICMSSN500", typeof(ICMSSN500))]
    [XmlElement("ICMSSN900", typeof(ICMSSN900))]
    public object Item { get; set; }
}
