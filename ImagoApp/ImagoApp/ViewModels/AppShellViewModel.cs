using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class AppShellViewModel : Util.BindableBase
    {
        private readonly ICharacterService _characterService;
        public ICommand GoToMainMenuCommand { get; }

        private bool _editMode;
        public event EventHandler<bool> EditModeChanged;

        public AppShellViewModel(ICharacterService characterService)
        {
            _characterService = characterService;

            GoToMainMenuCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    using (UserDialogs.Instance.Loading("Charakter wird gespeichert", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);
                        var result = await _characterService.SaveCharacter(App.CurrentCharacterViewModel.Character);
                        if (result)
                            await Device.InvokeOnMainThreadAsync(() =>
                            {
                                Xamarin.Forms.Application.Current.MainPage = new Views.StartPage();
                            });

                        await Task.Delay(250);
                    }
                });
            });
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
