using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    public class ProcRef
    {
        [XmlElement("nProc")]
        public string NProc { get; set; }

        [XmlElement("indProc")]
        public string IndProc { get; set; }
    }
}
