using System.Globalization;
using System.Xml.Serialization;

namespace NFe.Serialization
{
    public static class XmlSerializerHelper
    {
        public static string Serialize<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.portalfiscal.inf.br/nfe");

            using var sw = new StringWriter(CultureInfo.InvariantCulture);
            serializer.Serialize(sw, obj, ns);
            return sw.ToString();
        }
    }
}
