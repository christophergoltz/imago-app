using System;
using System.Linq;
using ImagoApp.ViewModels;
using ImagoApp.Views;
using ImagoApp.Views.CustomControls;
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
                    flyoutPageItem.IsSelected = true;
                    Detail = flyoutPageItem.NavigationPage;

                    //reset old selection
                    var oldItems = AppShellViewModel.MenuItems.Where(pageItem => pageItem != flyoutPageItem).ToList();
                    foreach (var oldItem in oldItems)
                    {
                        oldItem.IsSelected = false;
                    }
                }
            }
        }
    }
}