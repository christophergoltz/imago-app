using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Imago.Util;
using Imago.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class AppShellViewModel : BindableBase
    {
        public ICommand GoToMainMenuCommand { get; }

        private bool _editMode;
        private string _version;
        public event EventHandler<bool> EditModeChanged;

        public AppShellViewModel()
        {
            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;

            GoToMainMenuCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
            });
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
