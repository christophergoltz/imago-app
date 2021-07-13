﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
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
        private readonly CharacterInfoPageViewModel _characterInfoPageViewModel;
        private readonly SkillPageViewModel _skillPageViewModel;
        private readonly StatusPageViewModel _statusPageViewModel;
        private readonly InventoryViewModel _inventoryViewModel;
        private readonly WikiPageViewModel _wikiPageViewModel;
        public ICommand GoToMainMenuCommand { get; }
        private bool _editMode;
        private List<FlyoutPageItem> _menuItems;

        public List<FlyoutPageItem> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems ,value);
        }

        private List<FlyoutPageItem> CreateMainMenu()
        {
            var result = new List<FlyoutPageItem>()
            {
                new FlyoutPageItem("charakter_weiss.png", typeof(CharacterInfoPage),CreateNavigationPageForContent(new CharacterInfoPage(_characterInfoPageViewModel))),
                new FlyoutPageItem("vor_und_nachteile_weiss.png", typeof(PerksPage),CreateNavigationPageForContent(new PerksPage())),
                new FlyoutPageItem("weben_weiss.png", typeof(SkillPage),CreateNavigationPageForContent(new SkillPage(_skillPageViewModel))),
                new FlyoutPageItem("nahkampf_weiss.png", typeof(StatusPage),CreateNavigationPageForContent(new StatusPage(_statusPageViewModel))),
                new FlyoutPageItem("inventar_weiss.png", typeof(InventoryPage),CreateNavigationPageForContent(new InventoryPage(_inventoryViewModel))),
                new FlyoutPageItem("wiki_weiss.png", typeof(WikiPage),CreateNavigationPageForContent(new WikiPage(_wikiPageViewModel)))
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

        public AppShellViewModel(ICharacterService characterService,
            CharacterInfoPageViewModel characterInfoPageViewModel,
            SkillPageViewModel skillPageViewModel, StatusPageViewModel statusPageViewModel,
            InventoryViewModel inventoryViewModel, WikiPageViewModel wikiPageViewModel)
        {
            _characterService = characterService;
            _characterInfoPageViewModel = characterInfoPageViewModel;
            _skillPageViewModel = skillPageViewModel;
            _statusPageViewModel = statusPageViewModel;
            _inventoryViewModel = inventoryViewModel;
            _wikiPageViewModel = wikiPageViewModel;

            Device.BeginInvokeOnMainThread(() => { MenuItems = CreateMainMenu(); });

            GoToMainMenuCommand = new Command(() =>
            {
                bool saveResult= false;
                Task.Run(async () =>
                {
                    using (UserDialogs.Instance.Loading("Charakter wird gespeichert", null, null, true, MaskType.Black))
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
                                    });
                                }
                            }
                        });
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
