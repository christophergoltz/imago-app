using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Windows.Media.Audio;

namespace ImagoApp.Installer
{
    public static class CertificatHelper
    {
        public static void InstallCertificate(string certFile)
        {
            var t = X509Certificate.CreateFromCertFile(certFile);
           
            var store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);

            if (store.Certificates.Contains(t))
            {
                Console.WriteLine("Zertifikat ist bereits installiert");
            }
            else
            {
                Console.WriteLine("Zertifikat wird installiert..");
                var tz = new X509Certificate2(t);
                store.Add(tz);
                Console.WriteLine("Zertifikat erfolgreich installiert");
            }
        }
    }
}
