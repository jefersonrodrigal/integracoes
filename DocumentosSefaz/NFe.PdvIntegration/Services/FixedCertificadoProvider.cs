using System.Security.Cryptography.X509Certificates;
using NFe.PdvIntegration.Interfaces;

namespace NFe.PdvIntegration.Services;

public sealed class FixedCertificadoProvider : ICertificadoProvider
{
    private readonly X509Certificate2 _certificate;

    public FixedCertificadoProvider(X509Certificate2 certificate)
    {
        _certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
    }

    public X509Certificate2? ObterCertificado(string? empresaId = null)
    {
        return _certificate;
    }
}
