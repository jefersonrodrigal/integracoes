using Microsoft.Extensions.DependencyInjection;
using NFe.PdvIntegration.Configuration;
using NFe.PdvIntegration.Interfaces;
using NFe.PdvIntegration.Services;
using NFe.Signing.Interfaces;
using NFe.Signing.Services;

namespace NFe.PdvIntegration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNFePdvIntegration(
        this IServiceCollection services,
        Action<PdvIntegrationOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new PdvIntegrationOptions();
        configure?.Invoke(options);

        services.AddSingleton(options);
        services.AddTransient<IXmlSignatureService, XmlSignatureService>();
        services.AddTransient<IPdvNFeService, PdvNFeService>();

        return services;
    }
}
