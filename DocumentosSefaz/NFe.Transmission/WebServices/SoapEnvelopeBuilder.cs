using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Transmission.WebServices
{
    public static class SoapEnvelopeBuilder
    {
        public static string Build(string xml, string wsdl)
        {
            return $@"
<soap12:Envelope xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
  <soap12:Body>
    <nfeDadosMsg xmlns=""{wsdl}"">
      {xml}
    </nfeDadosMsg>
  </soap12:Body>
</soap12:Envelope>";
        }
    }

}
