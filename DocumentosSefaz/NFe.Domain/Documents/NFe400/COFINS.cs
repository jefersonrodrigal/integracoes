using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400;

public class COFINS
{
    [XmlElement("COFINSAliq", typeof(COFINSAliq))]
    [XmlElement("COFINSOutr", typeof(COFINSOutr))]
    public object Item { get; set; }
}

public class COFINSAliq
{
    public string CST { get; set; }
    public decimal vBC { get; set; }
    public decimal pCOFINS { get; set; }
    public decimal vCOFINS { get; set; }
}

public class COFINSOutr
{
    public string CST { get; set; }
    public decimal vBC { get; set; }
    public decimal pCOFINS { get; set; }
    public decimal vCOFINS { get; set; }
}
