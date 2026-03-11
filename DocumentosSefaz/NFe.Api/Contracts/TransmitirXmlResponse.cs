namespace NFe.Api.Contracts;

public sealed class TransmitirXmlResponse
{
    public bool Sucesso { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public bool EmContingencia { get; set; }
    public string XmlRetorno { get; set; } = string.Empty;
    public DateTimeOffset ProcessadoEm { get; set; } = DateTimeOffset.UtcNow;
}
