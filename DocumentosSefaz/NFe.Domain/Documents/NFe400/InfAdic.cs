using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    public class InfAdic
    {
        [XmlElement("infAdFisco")]
        public string InfAdFisco { get; set; }

        [XmlElement("infCpl")]
        public string InfCpl { get; set; }

        [XmlElement("obsCont")]
        public List<ObsCont> ObsCont { get; set; } = new();

        [XmlElement("obsFisco")]
        public List<ObsFisco> ObsFisco { get; set; } = new();

        [XmlElement("procRef")]
        public List<ProcRef> ProcRef { get; set; } = new();
    }
}
