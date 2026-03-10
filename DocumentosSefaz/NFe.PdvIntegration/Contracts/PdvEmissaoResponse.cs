namespace NFe.PdvIntegration.Contracts;

public sealed class PdvEmissaoResponse
{
    public bool Sucesso { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public DocumentoFiscalModelo Modelo { get; set; }
    public string XmlGerado { get; set; } = string.Empty;
    public string? XmlAssinado { get; set; }
    public PdvSefazRetorno? RetornoSefaz { get; set; }
    public DateTimeOffset ProcessadoEm { get; set; } = DateTimeOffset.UtcNow;
    public IReadOnlyList<string> Avisos { get; set; } = Array.Empty<string>();
}
