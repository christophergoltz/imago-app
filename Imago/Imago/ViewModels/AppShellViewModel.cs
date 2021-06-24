using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Imago.Util;
using Xamarin.Essentials;

namespace Imago.ViewModels
{
    public class AppShellViewModel : BindableBase
    {
        private bool _editMode;
        private string _version;
        public event EventHandler<bool> EditModeChanged;

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

        public bool EditMode
        {
            get => _editMode;
            set
            {
                SetProperty(ref _editMode, value);
                EditModeChanged?.Invoke(this, value);
            }
        }
    }
}
