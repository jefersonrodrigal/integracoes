using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    public class InfNFeSupl
    {
        [XmlElement("qrCode")]
        public string QrCode { get; set; }

        [XmlElement("urlChave")]
        public string UrlChave { get; set; }
    }
}
