using NFe.Transmission.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Transmission.Interfaces
{
    public interface ISefazHealthChecker
    {
        bool DeveAtivarContingencia(SefazResponse response);
    }
}
