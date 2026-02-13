using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Transmission.Response
{
    public class SefazResponse
    {
        public int? StatusCode { get; set; }
        public string Motivo { get; set; }
        public bool HouveFalhaComunicacao { get; set; }
    }
}
