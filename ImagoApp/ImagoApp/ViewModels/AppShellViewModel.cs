using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class AppShellViewModel : Util.BindableBase
    {
        private readonly Services.ICharacterService _characterService;
        public ICommand GoToMainMenuCommand { get; }

        private bool _editMode;
        private string _version;
        public event EventHandler<bool> EditModeChanged;

        public AppShellViewModel(Services.ICharacterService characterService)
        {
            _characterService = characterService;
            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;

            GoToMainMenuCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    using (UserDialogs.Instance.Loading("Charakter wird gespeichert", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);
                        var result = await _characterService.SaveCurrentCharacter();
                        if (result)
                            await Device.InvokeOnMainThreadAsync(() =>
                            {
                                Application.Current.MainPage = new Views.StartPage();
                            });
                            
                        await Task.Delay(250);
                    }
                });
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
