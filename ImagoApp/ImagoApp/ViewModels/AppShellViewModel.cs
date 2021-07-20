using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Application.Services;
using ImagoApp.Util;
using ImagoApp.Views;
using ImagoApp.Views.CustomControls;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class AppShellViewModel : BindableBase
    {
        public readonly CharacterInfoPageViewModel CharacterInfoPageViewModel;
        private readonly SkillPageViewModel _skillPageViewModel;
        private readonly StatusPageViewModel _statusPageViewModel;
        private readonly InventoryViewModel _inventoryViewModel;
        private readonly WikiPageViewModel _wikiPageViewModel;
        public ICommand GoToMainMenuCommand { get; }
        private bool _editMode;
        private List<FlyoutPageItem> _menuItems;

        public event EventHandler WikiPageOpenRequested;

        public void RaiseWikiPageOpenRequested()
        {
            WikiPageOpenRequested?.Invoke(this, EventArgs.Empty);
        }

        public List<FlyoutPageItem> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems ,value);
        }

        private List<FlyoutPageItem> CreateMainMenu()
        {
            var result = new List<FlyoutPageItem>()
            {
                new FlyoutPageItem("Images/charakter_weiss.png", typeof(CharacterInfoPage),CreateNavigationPageForContent(new CharacterInfoPage(CharacterInfoPageViewModel))),
                new FlyoutPageItem("Images/vor_und_nachteile_weiss.png", typeof(PerksPage),CreateNavigationPageForContent(new PerksPage())),
                new FlyoutPageItem("Images/weben_weiss.png", typeof(SkillPage),CreateNavigationPageForContent(new SkillPage(_skillPageViewModel))),
                new FlyoutPageItem("Images/nahkampf_weiss.png", typeof(StatusPage),CreateNavigationPageForContent(new StatusPage(_statusPageViewModel))),
                new FlyoutPageItem("Images/inventar_weiss.png", typeof(InventoryPage),CreateNavigationPageForContent(new InventoryPage(_inventoryViewModel))),
                new FlyoutPageItem("Images/wiki_weiss.png", typeof(WikiPage),CreateNavigationPageForContent(new WikiPage(_wikiPageViewModel)))
            };

            return result;
        }

        private NavigationPage CreateNavigationPageForContent(Page page)
        {
            var newDetail = new NavigationPage(page);
            Device.BeginInvokeOnMainThread(() =>
            {
                NavigationPage.SetHasNavigationBar(newDetail, false);
            });
            return newDetail;
        }

        public AppShellViewModel(CharacterInfoPageViewModel characterInfoPageViewModel,
            SkillPageViewModel skillPageViewModel, 
            StatusPageViewModel statusPageViewModel,
            InventoryViewModel inventoryViewModel, 
            WikiPageViewModel wikiPageViewModel)
        {
            CharacterInfoPageViewModel = characterInfoPageViewModel;
            _skillPageViewModel = skillPageViewModel;
            _statusPageViewModel = statusPageViewModel;
            _inventoryViewModel = inventoryViewModel;
            _wikiPageViewModel = wikiPageViewModel;

            Device.BeginInvokeOnMainThread(() => { MenuItems = CreateMainMenu(); });

            GoToMainMenuCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        bool saveResult = false;
                        using (UserDialogs.Instance.Loading("Charakter wird gespeichert", null, null, true,
                            MaskType.Black))
                        {
                            await Task.Delay(50);
                            saveResult = App.SaveCurrentCharacter();
                            await Task.Delay(50);
                        }

                        if (saveResult)
                        {
                            await Device.InvokeOnMainThreadAsync(() =>
                            {
                                Xamarin.Forms.Application.Current.MainPage = App.StartPage;

                                //clean up last ressources
                                App.StartPage.StartPageViewModel.RefreshData(true);
                            });
                        }
                        else
                        {
                            UserDialogs.Instance.Confirm(new ConfirmConfig
                            {
                                Message = $"{Environment.NewLine}Wie soll fortgefahren werden?" +
                                          $"{Environment.NewLine}{Environment.NewLine}      Abbrechen: Charakter bleibt geöffnet und speichern kann erneut versucht werden" +
                                          $"{Environment.NewLine}{Environment.NewLine}      Änderungen löschen: Die letzten Änderungen am Charakter werden nicht gespeichert und die Startseite wird geöffnet",
                                Title = "Fehler, der Charakter konnte nicht gespeichert werden",
                                OkText = "Abbrechen",
                                CancelText = "Änderungen löschen",
                                OnAction = result =>
                                {
                                    if (!result)
                                    {
                                        //user confirmed to go back anyway
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            Xamarin.Forms.Application.Current.MainPage = App.StartPage;

                                            //clean up last ressources
                                            App.StartPage.StartPageViewModel.RefreshData(true);
                                        });
                                    }
                                }
                            });
                        }
                    }
                    catch (Exception exception)
                    {
                        App.ErrorManager.TrackException(exception, CharacterInfoPageViewModel.CharacterViewModel.CharacterModel.Name);
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
