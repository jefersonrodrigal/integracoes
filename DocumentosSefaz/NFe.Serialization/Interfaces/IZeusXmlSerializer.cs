namespace NFe.Serialization.Interfaces;

public interface IZeusXmlSerializer
{
    string GerarXmlNFe(object nfeZeus);
    string GerarXmlNFCe(object nfceZeus);
}