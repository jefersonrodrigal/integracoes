using System.Security.Cryptography.X509Certificates;

namespace NFe.PdvIntegration.Interfaces;

public interface ICertificadoProvider
{
    X509Certificate2? ObterCertificado(string? empresaId = null);
}
