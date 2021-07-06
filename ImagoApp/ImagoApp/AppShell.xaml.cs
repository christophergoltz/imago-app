using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ImagoApp.Util;
using ImagoApp.ViewModels;
using Xamarin.Forms;

namespace ImagoApp
{
    public class FlyoutPageItem : BindableBase
    {
        private bool _isSelected;
        public string IconSource { get; set; }
        public NavigationPage NavigationPage { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }

    public partial class AppShell
    {
        public AppShellViewModel AppShellViewModel { get; private set; }

        public AppShell(AppShellViewModel appShellViewModel)
        {
            AppShellViewModel = appShellViewModel;
            this.BindingContext = AppShellViewModel;
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.SkillPage), typeof(Views.SkillPage));
            Routing.RegisterRoute(nameof(Views.StatusPage), typeof(Views.StatusPage));
            Routing.RegisterRoute(nameof(Views.InventoryPage), typeof(Views.InventoryPage));
            Routing.RegisterRoute(nameof(Views.WikiPage), typeof(Views.WikiPage));
            Routing.RegisterRoute(nameof(Views.ChangelogPage), typeof(Views.ChangelogPage));
            Routing.RegisterRoute(nameof(Views.PerksPage), typeof(Views.PerksPage));
        }

        //private void AppShell_OnNavigated(object sender, ShellNavigatedEventArgs e)
        //{
        //    if (sender is AppShell shell)
        //    {
        //        if (shell.CurrentPage is Views.WikiPage page)
        //        {
        //            if (page.BindingContext is ViewModels.WikiPageViewModel viewModel)
        //            {
        //                if (ViewModels.WikiPageViewModel.Instance == null)
        //                    ViewModels.WikiPageViewModel.Instance = viewModel;

        //                viewModel.OpenWikiPage();
        //            }
        //        }
        //    }
        //}

        private void AppShell_OnAppearing(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var flyoutPageItem = AppShellViewModel.MenuItems.First();
                flyoutPageItem.IsSelected = true;
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