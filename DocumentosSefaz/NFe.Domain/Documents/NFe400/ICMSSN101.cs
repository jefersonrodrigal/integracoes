using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    internal class ICMSSN101
    {
        [XmlElement("orig")]
        public string Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN { get; set; } = "101";

        [XmlElement("pCredSN")]
        public decimal PCredSN { get; set; }

        [XmlElement("vCredICMSSN")]
        public decimal VCredICMSSN { get; set; }
    }
}
