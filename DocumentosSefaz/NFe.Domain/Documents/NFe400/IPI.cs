using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400;

public class IPI
{
    public string cEnq { get; set; }

    [XmlElement("IPITrib", typeof(IPITrib))]
    [XmlElement("IPINT", typeof(IPINT))]
    public object Item { get; set; }
}

public class IPITrib
{
    public string CST { get; set; }
    public decimal vBC { get; set; }
    public decimal pIPI { get; set; }
    public decimal vIPI { get; set; }
}

public class IPINT
{
    public string CST { get; set; }
}
