namespace NFe.PdvIntegration.Contracts;

public sealed class PdvEmissaoRequest
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");
    public string? EmpresaId { get; set; }
    public required object DocumentoZeus { get; set; }
    public DocumentoFiscalModelo Modelo { get; set; }
    public bool? AssinarXml { get; set; }
    public bool? EnviarParaSefaz { get; set; }
}
