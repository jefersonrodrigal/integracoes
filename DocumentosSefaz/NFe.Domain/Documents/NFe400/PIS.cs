using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400;

public class PIS
{
    [XmlElement("PISAliq", typeof(PISAliq))]
    [XmlElement("PISOutr", typeof(PISOutr))]
    public object Item { get; set; }
}

public class PISAliq
{
    public string CST { get; set; }
    public decimal vBC { get; set; }
    public decimal pPIS { get; set; }
    public decimal vPIS { get; set; }
}

public class PISOutr
{
    public string CST { get; set; }
    public decimal vBC { get; set; }
    public decimal pPIS { get; set; }
    public decimal vPIS { get; set; }
}
