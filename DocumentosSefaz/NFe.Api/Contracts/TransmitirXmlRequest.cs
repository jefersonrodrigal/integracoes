using NFe.PdvIntegration.Contracts;

namespace NFe.Api.Contracts;

public sealed class TransmitirXmlRequest
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");
    public DocumentoFiscalModelo Modelo { get; set; }
    public string XmlAssinado { get; set; } = string.Empty;
}
