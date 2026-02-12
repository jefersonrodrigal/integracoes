using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace NFe.Validation;

public class XmlSchemaValidator
{
    private readonly XmlSchemaSet _schemas;

    public XmlSchemaValidator(string schemaFolderPath)
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
}
