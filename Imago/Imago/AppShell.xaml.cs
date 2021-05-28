using Imago.ViewModels;
using Imago.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Imago
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(CharacterInfoPage), typeof(CharacterInfoPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//LoginPage");
        }
    }
}
