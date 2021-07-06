using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application.Services;
using ImagoApp.Views;
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

        public List<FlyoutPageItem> MenuItems { get; set; }
        
        public AppShellViewModel(ICharacterService characterService)
        {
            _characterService = characterService;
            MenuItems = new List<FlyoutPageItem>()
            {
                new FlyoutPageItem()
                    {Title = "aw", IconSource = "charakter_weiss.png", TargetType = typeof(CharacterInfoPage)},
                new FlyoutPageItem()
                    {Title = "aw", IconSource = "vor_und_nachteile_weiss.png", TargetType = typeof(PerksPage)},
                new FlyoutPageItem() {Title = "aw", IconSource = "weben_weiss.png", TargetType = typeof(SkillPage)},
                new FlyoutPageItem() {Title = "aw", IconSource = "nahkampf_weiss.png", TargetType = typeof(StatusPage)},
                new FlyoutPageItem() {Title = "aw", IconSource = "inventar_weiss.png", TargetType = typeof(InventoryPage)},
                new FlyoutPageItem() {Title = "aw", IconSource = "wiki_weiss.png", TargetType = typeof(WikiPage)}
            };

            GoToMainMenuCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    using (UserDialogs.Instance.Loading("Charakter wird gespeichert", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(250);
                        var result = _characterService.SaveCharacter(App.CurrentCharacterViewModel.Character);
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
