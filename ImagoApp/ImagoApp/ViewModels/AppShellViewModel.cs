using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application.Services;
using ImagoApp.Util;
using ImagoApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class AppShellViewModel : BindableBase
    {
        private readonly ICharacterService _characterService;
        public ICommand GoToMainMenuCommand { get; }
        private bool _editMode;

        public List<FlyoutPageItem> MenuItems { get; set; }

        private List<FlyoutPageItem> CreateMainMenu()
        {
            var result = new List<FlyoutPageItem>()
            {
                new FlyoutPageItem()
                {
                    IconSource = "charakter_weiss.png",
                    NavigationPage = CreateNavigationPageForContent(typeof(CharacterInfoPage))
                },
                new FlyoutPageItem()
                {
                    IconSource = "vor_und_nachteile_weiss.png",
                    NavigationPage = CreateNavigationPageForContent(typeof(PerksPage))
                },
                new FlyoutPageItem()
                {
                    IconSource = "weben_weiss.png",
                    NavigationPage = CreateNavigationPageForContent(typeof(SkillPage))
                },
                new FlyoutPageItem()
                {
                    IconSource = "nahkampf_weiss.png",
                    NavigationPage = CreateNavigationPageForContent(typeof(StatusPage))
                },
                new FlyoutPageItem()
                {
                    IconSource = "inventar_weiss.png",
                    NavigationPage = CreateNavigationPageForContent(typeof(InventoryPage))
                },
                new FlyoutPageItem()
                {
                    IconSource = "wiki_weiss.png",
                    NavigationPage = CreateNavigationPageForContent(typeof(WikiPage))
                }
            };

            return result;
        }

        private NavigationPage CreateNavigationPageForContent(Type pageType)
        {
            var newDetail = new NavigationPage((Page) Activator.CreateInstance(pageType));
            NavigationPage.SetHasNavigationBar(newDetail, false);
            return newDetail;
        }

        public AppShellViewModel(ICharacterService characterService)
        {
            _characterService = characterService;
            MenuItems = CreateMainMenu();
     
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
                App.CurrentCharacterViewModel.EditMode = value;
            }
        }
    }
}
