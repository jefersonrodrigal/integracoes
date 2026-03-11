namespace NFe.Api.Configuration;

public sealed class SefazTransmissionOptions
{
    public string NFeAutorizacaoUrl { get; set; } = "https://homologacao.nfe.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx";
    public string NFCeAutorizacaoUrl { get; set; } = "https://homologacao.nfce.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx";
}
