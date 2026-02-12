using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Infrastructure.Configuration
{
    public sealed class NFeEnvironmentConfig
    {
        public NFe.Domain.Enuns.Environment Ambiente { get; init; }
        public string XsdPath { get; init; } = default!;
        public string UrlAutorizacao { get; init; } = default!;
        public string UrlRetorno { get; init; } = default!;
    }
}
