using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace NFe.Signing.Interfaces
{
    public interface IXmlSignatureService
    {
        XmlDocument Sign(XmlDocument xml, X509Certificate2 certificate);
    }
}
