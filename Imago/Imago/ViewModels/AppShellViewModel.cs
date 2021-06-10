using System;
using System.Collections.Generic;
using System.Text;
using Imago.Shared.Util;
using Imago.Util;
using Xamarin.Essentials;

namespace Imago.ViewModels
{
    public class AppShellViewModel : BindableBase
    {
        private string _version;

        public AppShellViewModel()
        {
            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;
        }

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }
    }
}
