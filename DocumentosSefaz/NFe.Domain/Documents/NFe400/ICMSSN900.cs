using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    internal class ICMSSN900
    {
        [XmlElement("orig")]
        public string Orig { get; set; }

        [XmlElement("CSOSN")]
        public string CSOSN { get; set; } = "900";

        [XmlElement("modBC")]
        public string ModBC { get; set; }

        [XmlElement("vBC")]
        public decimal? VBC { get; set; }

        [XmlElement("pRedBC")]
        public decimal? PRedBC { get; set; }

        [XmlElement("pICMS")]
        public decimal? PICMS { get; set; }

        [XmlElement("vICMS")]
        public decimal? VICMS { get; set; }

        [XmlElement("modBCST")]
        public string ModBCST { get; set; }

        [XmlElement("pMVAST")]
        public decimal? PMVAST { get; set; }

        [XmlElement("pRedBCST")]
        public decimal? PRedBCST { get; set; }

        [XmlElement("vBCST")]
        public decimal? VBCST { get; set; }

        [XmlElement("pICMSST")]
        public decimal? PICMSST { get; set; }

        [XmlElement("vICMSST")]
        public decimal? VICMSST { get; set; }

        [XmlElement("pCredSN")]
        public decimal? PCredSN { get; set; }

        [XmlElement("vCredICMSSN")]
        public decimal? VCredICMSSN { get; set; }
    }
}
