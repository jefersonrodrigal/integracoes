namespace NFe.Transmission.Results;

public class NFeSendResult
{
    public bool Sucesso { get; set; }
    public bool EmContingencia { get; set; }
    public string Codigo { get; set; }
    public string Mensagem { get; set; }
    public string XmlAutorizado { get; set; }
}
