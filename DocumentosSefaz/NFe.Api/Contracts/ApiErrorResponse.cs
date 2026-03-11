namespace NFe.Api.Contracts;

public sealed class ApiErrorResponse
{
    public string CorrelationId { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string? Detalhes { get; set; }
    public DateTimeOffset ProcessadoEm { get; set; } = DateTimeOffset.UtcNow;
}
