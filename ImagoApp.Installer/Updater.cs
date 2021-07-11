using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Windows.Devices.Bluetooth.Advertisement;
using System.Net;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;
using Windows.Management.Deployment;

namespace ImagoApp.Installer
{
    public static class Updater
    {
        private const string ImagoAppName = "Imago.App";

        public static void DownloadLatestRelease(string url, string downloadFile)
        {
            using WebClient webClient = new WebClient();
            webClient.DownloadFile(url, downloadFile);
        }

        public static void PromptInstallation(string installBundleFile)
        {
            Process.Start("cmd.exe", $"/c \"{installBundleFile}\"");
        }

        public static Version GetInstalledAppVersion()
        {
            var allInstalledApps = new PackageManager().FindPackages();

            foreach (var package in allInstalledApps)
            {
                if (package.DisplayName.Equals(ImagoAppName))
                {
                    var version = new Version(package.Id.Version.Major, package.Id.Version.Minor, package.Id.Version.Build, package.Id.Version.Revision);
                    return version;
                }
            }
            return null;
        }
    }
}
