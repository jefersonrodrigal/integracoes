using NFe.Serialization.Interfaces;
using NFe.Serialization.Zeus;

namespace NFe.Builders;

public class NFCeBuilder
{
    private readonly IZeusXmlSerializer _serializer;

    public NFCeBuilder(IZeusXmlSerializer? serializer = null)
    {
        _serializer = serializer ?? new ZeusXmlSerializer();
    }

    public string GerarXml(object nfceZeus)
    {
        return _serializer.GerarXmlNFCe(nfceZeus);
    }
}