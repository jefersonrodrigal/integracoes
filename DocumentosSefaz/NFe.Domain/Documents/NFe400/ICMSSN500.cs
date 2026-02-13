using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    internal class ICMSSN500
    {
        [XmlElement("orig")]
        public string Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN { get; set; } = "500";

        [XmlElement("vBCSTRet")]
        public decimal? VBCSTRet { get; set; }

        [XmlElement("pST")]
        public decimal? PST { get; set; }

        [XmlElement("vICMSSTRet")]
        public decimal? VICMSSTRet { get; set; }
    }
}
