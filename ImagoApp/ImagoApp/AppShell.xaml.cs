using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Util;
using ImagoApp.ViewModels;
using Xamarin.Forms;

namespace ImagoApp
{
    public class FlyoutPageItem : BindableBase
    {
        public FlyoutPageItem(string iconSource, Type pageType, NavigationPage page)
        {
            IconSource = iconSource;
            PageType = pageType;
            NavigationPage = page;
        }

        private bool _isSelected;
        public string IconSource { get; set; }
        public NavigationPage NavigationPage { get; set; }
        public Type PageType { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }

    public partial class AppShell
    {
        public AppShellViewModel AppShellViewModel { get; }

        public AppShell(AppShellViewModel appShellViewModel)
        {
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