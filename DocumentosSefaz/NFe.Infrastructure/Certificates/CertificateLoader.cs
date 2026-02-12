using System;
using System.Collections.Generic;
using System.Text;

namespace NFe.Infrastructure.Certificates
{
    internal class CertificateLoader
    {
        /// <summary>
        /// Carrega um certificado digital A1 a partir de um arquivo PFX.
        /// </summary>
        public static System.Security.Cryptography.X509Certificates.X509Certificate2 LoadFromFile(string pfxPath, string password)
        {
            return new System.Security.Cryptography.X509Certificates.X509Certificate2(pfxPath, password, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet | System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.PersistKeySet);
        }

        /// <summary>
        /// Carrega um certificado digital do repositório do Windows pelo Subject Name.
        /// </summary>
        public static System.Security.Cryptography.X509Certificates.X509Certificate2? LoadFromStoreBySubject(string subjectName, System.Security.Cryptography.X509Certificates.StoreLocation storeLocation = System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser)
        {
            using var store = new System.Security.Cryptography.X509Certificates.X509Store(System.Security.Cryptography.X509Certificates.StoreName.My, storeLocation);
            store.Open(System.Security.Cryptography.X509Certificates.OpenFlags.ReadOnly);
            foreach (var cert in store.Certificates)
            {
                if (cert.SubjectName.Name != null && cert.SubjectName.Name.Contains(subjectName, StringComparison.OrdinalIgnoreCase))
                {
                    return cert;
                }
            }
            return null;
        }

        /// <summary>
        /// Carrega um certificado digital do repositório do Windows pelo Thumbprint.
        /// </summary>
        public static System.Security.Cryptography.X509Certificates.X509Certificate2? LoadFromStoreByThumbprint(string thumbprint, System.Security.Cryptography.X509Certificates.StoreLocation storeLocation = System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser)
        {
            using var store = new System.Security.Cryptography.X509Certificates.X509Store(System.Security.Cryptography.X509Certificates.StoreName.My, storeLocation);
            store.Open(System.Security.Cryptography.X509Certificates.OpenFlags.ReadOnly);
            foreach (var cert in store.Certificates)
            {
                if (string.Equals(cert.Thumbprint?.Replace(" ", string.Empty), thumbprint.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase))
                {
                    return cert;
                }
            }
            return null;
        }
    }
}
