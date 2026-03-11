using NFe.PdvIntegration.Contracts;

namespace NFe.Api.Contracts;

public sealed class AssinarEEnviarXmlRequest
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");
    public string? EmpresaId { get; set; }
    public DocumentoFiscalModelo Modelo { get; set; }
    public string Xml { get; set; } = string.Empty;
}
