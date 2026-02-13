using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    public class ObsCont
    {
        [XmlAttribute("xCampo")]
        public string XCampo { get; set; }

        [XmlElement("xTexto")]
        public string XTexto { get; set; }
    }
}
