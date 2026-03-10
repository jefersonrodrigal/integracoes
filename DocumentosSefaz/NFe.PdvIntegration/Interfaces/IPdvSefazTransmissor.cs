using NFe.PdvIntegration.Contracts;

namespace NFe.PdvIntegration.Interfaces;

public interface IPdvSefazTransmissor
{
    Task<PdvSefazRetorno> EnviarAsync(string xmlAssinado, DocumentoFiscalModelo modelo, CancellationToken cancellationToken = default);
}
