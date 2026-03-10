using NFe.PdvIntegration.Contracts;

namespace NFe.PdvIntegration.Interfaces;

public interface IPdvNFeService
{
    Task<PdvEmissaoResponse> EmitirAsync(PdvEmissaoRequest request, CancellationToken cancellationToken = default);
}
