using NFe.Transmission.Contingencia;
using NFe.Transmission.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Transmission.Interfaces
{
    public interface IContingenciaFactory
    {
        IContingenciaStrategy Criar(SefazResponse response);
    }
}
