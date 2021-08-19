using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ImagoApp.ViewModels;
using ImagoApp.Views;
using ImagoApp.Views.CustomControls;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace ImagoApp
{
    public partial class AppShell
    {
        public AppShellViewModel AppShellViewModel { get; }

        public AppShell(AppShellViewModel appShellViewModel)
        {
            appShellViewModel.WikiPageOpenRequested += (sender, args) =>
            {
                var wikiPage = AppShellViewModel.MenuItems.First(item => item.PageType == typeof(WikiPage));
                //direct access to listview is required to set selection accordingly to ui
                MainMenuCollectionView.SelectedItem = wikiPage;
            };
            BindingContext = AppShellViewModel = appShellViewModel;
            InitializeComponent();
        }

        private void AppShell_OnAppearing(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var flyoutPageItem = AppShellViewModel.MenuItems.First();
                //direct access to listview is required to set selection accordingly to ui
                MainMenuCollectionView.SelectedItem = flyoutPageItem;
            });
        }

        private void SelectableItemsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Any())
            {
                if (e.CurrentSelection.First() is FlyoutPageItem flyoutPageItem)
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

                            if (!saveResult)
                            {
                                UserDialogs.Instance.Alert(
                                    $"{Environment.NewLine}Der Charakter konnte aus einem unbekannten Grund nicht gespeichert werden." +
                                    $"{Environment.NewLine}{Environment.NewLine}Möglicherweise kann ein neustarten der App das Problem lösen.",
                                    "Fehler, der Charakter konnte nicht gespeichert werden",
                                    "OK");
                            }
                        }
                        catch (Exception exception)
                        {
                            App.ErrorManager.TrackException(exception,
                                AppShellViewModel.CharacterInfoPageViewModel.CharacterViewModel.CharacterModel.Name);
                        }

                        Analytics.TrackEvent("Switch detailpage", new Dictionary<string, string>
                        {
                            {"Page", flyoutPageItem.NavigationPage.CurrentPage.GetType().ToString() }
                        });

                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            flyoutPageItem.IsSelected = true;
                            Detail = flyoutPageItem.NavigationPage;

                            //reset old selection
                            var oldItems = AppShellViewModel.MenuItems.Where(pageItem => pageItem != flyoutPageItem)
                                .ToList();
                            foreach (var oldItem in oldItems)
                            {
                                oldItem.IsSelected = false;
                            }
                        });

                        if (flyoutPageItem.NavigationPage.CurrentPage is WeaveTalentPage weaveTalentPage)
                        {
                            weaveTalentPage.ViewModel.InitializeWeaveTalentList();
                        }
                    });
                }
            }
        }
    }
}