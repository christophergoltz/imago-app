using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Manager;
using ImagoApp.Views;
using ImagoApp.Views.CustomControls;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class AppShellViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        public readonly CharacterInfoPageViewModel CharacterInfoPageViewModel;
        private readonly SkillPageViewModel _skillPageViewModel;
        private readonly StatusPageViewModel _statusPageViewModel;
        private readonly InventoryViewModel _inventoryViewModel;
        private readonly WikiPageViewModel _wikiPageViewModel;
        private readonly WeaveTalentPageViewModel _weaveTalentPageViewModel;
        private readonly DicePageViewModel _dicePageViewModel;
        private readonly ICharacterProvider _characterProvider;
        public ICommand GoToMainMenuCommand { get; }
        private List<FlyoutPageItem> _menuItems;

        public event EventHandler<Type> SwitchPageRequested;

        public void RaiseSwitchPageRequested(Type requestedPage)
        {
            SwitchPageRequested?.Invoke(this, requestedPage);
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
                new FlyoutPageItem("Images/fertigkeit_weiss.png", typeof(SkillPage),CreateNavigationPageForContent(new SkillPage(_skillPageViewModel))),
                new FlyoutPageItem("Images/kampf_weiss.png", typeof(StatusPage),CreateNavigationPageForContent(new StatusPage(_statusPageViewModel))),
                new FlyoutPageItem("Images/inventar_weiss.png", typeof(InventoryPage),CreateNavigationPageForContent(new InventoryPage(_inventoryViewModel))),
                new FlyoutPageItem("Images/wiki_weiss.png", typeof(WikiPage),CreateNavigationPageForContent(new WikiPage(_wikiPageViewModel))),
                new FlyoutPageItem("Images/weben_weiss.png", typeof(WeaveTalentPage),CreateNavigationPageForContent(new WeaveTalentPage(_weaveTalentPageViewModel))),
                new FlyoutPageItem("Images/wuerfel_weiss.png", typeof(DicePage),CreateNavigationPageForContent(new DicePage(_dicePageViewModel)))
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

        public AppShellViewModel(CharacterViewModel characterViewModel,
            CharacterInfoPageViewModel characterInfoPageViewModel,
            SkillPageViewModel skillPageViewModel, 
            StatusPageViewModel statusPageViewModel,
            InventoryViewModel inventoryViewModel, 
            WikiPageViewModel wikiPageViewModel,
            WeaveTalentPageViewModel weaveTalentPageViewModel,
            DicePageViewModel dicePageViewModel,
            ICharacterProvider characterProvider)
        {
            _characterViewModel = characterViewModel;
            CharacterInfoPageViewModel = characterInfoPageViewModel;
            _skillPageViewModel = skillPageViewModel;
            _statusPageViewModel = statusPageViewModel;
            _inventoryViewModel = inventoryViewModel;
            _wikiPageViewModel = wikiPageViewModel;
            _weaveTalentPageViewModel = weaveTalentPageViewModel;
            _dicePageViewModel = dicePageViewModel;
            _characterProvider = characterProvider;

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
                            Analytics.TrackEvent("Close Character");
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
                                App.StartPage.StartPageViewModel.RefreshData(false, true, true);
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
                                            App.StartPage.StartPageViewModel.RefreshData(false, true, true);
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
            get => _characterViewModel.EditMode;
            set
            {
                var characterViewModel = _characterProvider.CurrentCharacter;

                //check if value is not set by user
                if (value && _characterViewModel.EditMode)
                    return;

                Analytics.TrackEvent("Edit mode changed",
                    new Dictionary<string, string>() {{"Value", value.ToString()}});

                if (!value)
                {
                    //disable
                    characterViewModel.EditMode = false;
                    OnPropertyChanged(nameof(EditMode));
                }
                else
                {
                    //display warning
                    Task.Run(async () =>
                    {
                        var result =
                            await UserDialogs.Instance.ConfirmAsync(
                                $"{Environment.NewLine}Soll der Bearbeitungs-Modus für den Charakter wirklich aktiviert werden?" +
                                $"{Environment.NewLine}{Environment.NewLine}In diesem Modus kann ggf. durch entsprechende Veränderung des Charakters eine korrekte Berechnung der App nicht mehr gewährleistet werden",
                                "Bearbeitungs-Modus für den Charakter aktvieren", "Ja", "Abbrechen");
                        if (result)
                        {
                            //user confirmed
                            characterViewModel.EditMode = true;
                            OnPropertyChanged(nameof(EditMode));
                        }
                        else
                        {
                            //user cancelled
                            OnPropertyChanged(nameof(EditMode));
                        }
                    });
                }
            }
        }

        public void RaiseEditModeChanged()
        {
            OnPropertyChanged(nameof(EditMode));
        }
    }
}
