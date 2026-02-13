using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    public class Transp
    {
        [XmlElement("modFrete")]
        public string ModFrete { get; set; }

        public Transporta Transporta { get; set; }

        [XmlElement("vol")]
        public List<Vol> Vol { get; set; }
    }
}
