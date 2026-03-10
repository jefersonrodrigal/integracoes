using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NFe.Serialization.Interfaces;

namespace NFe.Serialization.Zeus;

public class ZeusXmlSerializer : IZeusXmlSerializer
{
    private const string NfeNamespace = "http://www.portalfiscal.inf.br/nfe";

    public string GerarXmlNFe(object nfeZeus)
    {
        ValidarDocumentoZeus(nfeZeus, "55");
        return SerializarComNamespaceNFe(nfeZeus);
    }

    public string GerarXmlNFCe(object nfceZeus)
    {
        ValidarDocumentoZeus(nfceZeus, "65");
        return SerializarComNamespaceNFe(nfceZeus);
    }

    private static string SerializarComNamespaceNFe(object documento)
    {
        var serializer = new XmlSerializer(documento.GetType());
        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, NfeNamespace);

        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false,
            Indent = true
        };

        using var writer = new Utf8StringWriter();
        using var xmlWriter = XmlWriter.Create(writer, settings);
        serializer.Serialize(xmlWriter, documento, ns);

        return writer.ToString();
    }

    private static void ValidarDocumentoZeus(object documento, string modeloEsperado)
    {
        ArgumentNullException.ThrowIfNull(documento);

        var assemblyName = documento.GetType().Assembly.GetName().Name;
        if (string.IsNullOrWhiteSpace(assemblyName) || !assemblyName.StartsWith("NFe.", StringComparison.Ordinal))
        {
            throw new ArgumentException("O objeto informado não pertence à biblioteca Zeus.Net.NFe.NFCe.");
        }

        var modeloEncontrado = ObterModeloFiscal(documento);
        if (!string.IsNullOrWhiteSpace(modeloEncontrado) && modeloEncontrado != modeloEsperado)
        {
            throw new ArgumentException($"Documento incompatível com o modelo esperado. Esperado: {modeloEsperado}, encontrado: {modeloEncontrado}.");
        }
    }

    private static string? ObterModeloFiscal(object documento)
    {
        var infNfe = GetPropertyValue(documento, "infNFe");
        var ide = infNfe is null ? null : GetPropertyValue(infNfe, "ide");

        var mod = ide is null ? null : GetPropertyValue(ide, "mod");
        return mod?.ToString();
    }

    private static object? GetPropertyValue(object instance, string propertyName)
    {
        var property = instance
            .GetType()
            .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        return property?.GetValue(instance);
    }

    private sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}