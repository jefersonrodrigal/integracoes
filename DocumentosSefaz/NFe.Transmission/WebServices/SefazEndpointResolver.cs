using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Transmission.WebServices
{
    public class SefazEndpointResolver
    {
        public string ObterUrlAutorizacao(int cUF, int tpAmb)
        {
            if (tpAmb == 2)
                return "https://homologacao.nfe.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx";

            return "https://nfe.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx";
        }
    }

}
