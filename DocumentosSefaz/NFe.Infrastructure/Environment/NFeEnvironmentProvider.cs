using NFe.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NFe.Infrastructure.Environment
{
    public static class NFeEnvironmentProvider
    {
        public static NFeEnvironmentConfig Get(NFe.Domain.Enuns.Environment ambiente)
        {
            return GetAsync(ambiente).GetAwaiter().GetResult();
        }

        public static Task<NFeEnvironmentConfig> GetAsync(NFe.Domain.Enuns.Environment ambiente)
        {
            var config = ambiente switch
            {
                NFe.Domain.Enuns.Environment.Homologacao => new NFeEnvironmentConfig()
                {
                    Ambiente = NFe.Domain.Enuns.Environment.Homologacao,
                    XsdPath = "Schemas/Homologacao",
                    UrlAutorizacao = "https://homologacao.nfe.sefaz..."
                },

                NFe.Domain.Enuns.Environment.Producao => new NFeEnvironmentConfig()
                {
                    Ambiente = NFe.Domain.Enuns.Environment.Producao,
                    XsdPath = "Schemas/Producao",
                    UrlAutorizacao = "https://nfe.sefaz..."
                },

                _ => throw new ArgumentOutOfRangeException(nameof(ambiente))
            };

            return Task.FromResult(config);
        }
    }
}
