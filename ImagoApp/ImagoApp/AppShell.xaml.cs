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
                SetSelectedPage(wikiPage);
            };
            BindingContext = AppShellViewModel = appShellViewModel;
            InitializeComponent();
        }
        
        private void AppShell_OnAppearing(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var flyoutPageItem = AppShellViewModel.MenuItems.First();
                SetSelectedPage(flyoutPageItem);
            });
        }

        private void SelectableItemsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Any())
            {
                if (e.CurrentSelection.First() is FlyoutPageItem flyoutPageItem)
                {
                    SetSelectedPage(flyoutPageItem);
                }
            }
        }

        private void SetSelectedPage(FlyoutPageItem flyoutPageItem)
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