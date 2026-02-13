using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace NFe.Domain.Documents.NFe400
{
    public class Vol
    {
        [XmlElement("qVol")]
        public decimal? QVol { get; set; }

        [XmlElement("esp")]
        public string Esp { get; set; }

        [XmlElement("marca")]
        public string Marca { get; set; }

        [XmlElement("nVol")]
        public string NVol { get; set; }

        [XmlElement("pesoL")]
        public decimal? PesoL { get; set; }

        [XmlElement("pesoB")]
        public decimal? PesoB { get; set; }
    }
}
