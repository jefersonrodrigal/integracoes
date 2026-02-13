using System.Xml;
using System.Xml.Schema;

namespace NFe.Validation;

public class XmlSchemaValidate
{
    private readonly XmlSchemaSet _schemas;

    public XmlSchemaValidate(string schemaFolderPath)
    {
        _schemas = new XmlSchemaSet();

        foreach (var file in Directory.GetFiles(schemaFolderPath, "*.xsd"))
        {
            _schemas.Add(null, file);
        }
    }

    public List<string> Validate(string xmlContent)
    {
        var errors = new List<string>();

        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Schemas = _schemas
        };

        settings.ValidationEventHandler += (sender, args) =>
        {
            errors.Add(args.Message);
        };

        using var reader = XmlReader.Create(new StringReader(xmlContent), settings);
        while (reader.Read()) { }

        return errors;
    }

    public void ValidateOrThrow(string xmlContent)
    {
        var errors = Validate(xmlContent);

        if (errors.Any())
        {
            throw new XmlSchemaValidationException(
                "Erro de validação XSD: " + string.Join(" | ", errors));
        }
    }
}
