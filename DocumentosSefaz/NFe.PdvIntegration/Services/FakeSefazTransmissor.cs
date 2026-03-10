using NFe.PdvIntegration.Contracts;
using NFe.PdvIntegration.Interfaces;

namespace NFe.PdvIntegration.Services;

public sealed class FakeSefazTransmissor : IPdvSefazTransmissor
{
    public Task<PdvSefazRetorno> EnviarAsync(string xmlAssinado, DocumentoFiscalModelo modelo, CancellationToken cancellationToken = default)
    {
        var retorno = new PdvSefazRetorno
        {
            Sucesso = true,
            Codigo = "100",
            Mensagem = "Autorizado uso do documento fiscal (simulado).",
            EmContingencia = false,
            XmlRetorno = xmlAssinado
        };

        return Task.FromResult(retorno);
    }
}
