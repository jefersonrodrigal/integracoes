namespace NFe.PdvIntegration.Configuration;

public sealed class PdvIntegrationOptions
{
    public bool AssinarXmlPorPadrao { get; set; } = true;
    public bool EnviarParaSefazPorPadrao { get; set; }
    public bool ExigirCertificadoParaAssinatura { get; set; } = true;
}
