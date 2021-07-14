using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application;
using Xamarin.Forms;

namespace ImagoApp.Views.CustomControls
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

}
