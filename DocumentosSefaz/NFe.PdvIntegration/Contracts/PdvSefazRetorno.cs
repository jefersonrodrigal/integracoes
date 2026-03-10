namespace NFe.PdvIntegration.Contracts;

public sealed class PdvSefazRetorno
{
    public bool Sucesso { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public bool EmContingencia { get; set; }
    public string? XmlRetorno { get; set; }
}
