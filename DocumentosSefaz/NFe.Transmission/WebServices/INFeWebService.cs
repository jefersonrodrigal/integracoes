using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Transmission.WebServices
{
    public interface INFeWebService
    {
        Task<string> EnviarAutorizacaoAsync(string xml);
        Task<string> EnviarEventoAsync(string xml);
        Task<string> EnviarInutilizacaoAsync(string xml);
    }

}
