using NFe.Transmission.Interfaces;
using NFe.Transmission.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Transmission
{
    public class SefazHealthChecker : ISefazHealthChecker
    {
        public bool DeveAtivarContingencia(SefazResponse response)
        {
            if (response == null)
                return true;

            if (response.HouveFalhaComunicacao)
                return true;

            if (response.StatusCode == 108 || response.StatusCode == 109)
                return true;

            return false;
        }
    }
}
