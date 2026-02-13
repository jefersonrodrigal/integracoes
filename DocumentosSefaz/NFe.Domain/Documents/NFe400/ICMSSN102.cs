using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    internal class ICMSSN102
    {
        [XmlElement("orig")]
        public string Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN { get; set; } = "102";
    }
}
