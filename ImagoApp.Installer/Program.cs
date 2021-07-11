using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using Windows.Storage;
using ImagoApp.Infrastructure.Repositories;
using Newtonsoft.Json;

namespace ImagoApp.Installer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var githubUpdateRepository = new GithubUpdateRepository();

            Console.WriteLine("Installierte App-Version wird abgerufen");
            //check if app is installed or update
            var installedVersion = Updater.GetInstalledAppVersion();

            if(installedVersion == null)
                Console.WriteLine("Kein Imago.App gefunden");
            else
                Console.WriteLine("Imago.App mit Version " + installedVersion + " gefunden");

            var latestVersionAvaiable = githubUpdateRepository.GetLatestRelease();
            var latestVersionAvaiableVersion = new Version(latestVersionAvaiable.tag_name);

            if (installedVersion == null || latestVersionAvaiableVersion > installedVersion)
            {
                if(latestVersionAvaiableVersion > installedVersion)
                    Console.WriteLine("Neue Version online gefunden: " + latestVersionAvaiableVersion);
                
                var downloadUrl = latestVersionAvaiable.assets[0].browser_download_url;
                Console.WriteLine("Download Url gefunden: " + downloadUrl);

                var downloadFolder = FolderHelper.GetDownloadsPath();
                var downloadFileName = Path.Combine(downloadFolder,
                    "Imago.App_" + latestVersionAvaiableVersion + ".zip");
                
                //download
                Console.WriteLine("Aktuellste Version wird heruntergeladen..");
                Updater.DownloadLatestRelease(downloadUrl, downloadFileName);

                string msixBundleFile = "";
                string certFile = "";

                using (var archive = ZipFile.OpenRead(downloadFileName))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(".msixbundle"))
                            msixBundleFile = Path.Combine(downloadFolder, entry.FullName);

                        if (entry.FullName.EndsWith(".cer"))
                            certFile = Path.Combine(downloadFolder, entry.FullName);
                    }
                }

                //extract
                Console.WriteLine("Download wird entpackt");
                ZipFile.ExtractToDirectory(downloadFileName, downloadFolder, true);

                Console.WriteLine("Installer: " + msixBundleFile);
                Console.WriteLine("Zertifikat: " + certFile);
                
                //check cert
                Console.WriteLine("Zertifikat wird überprüft");
                CertificatHelper.InstallCertificate(certFile);

                //install
                Console.WriteLine("Installation wird geöffnet..");
                Updater.PromptInstallation(msixBundleFile);
            }
        }
    }
}
