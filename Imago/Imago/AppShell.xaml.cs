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
            Routing.RegisterRoute(nameof(SkillPage), typeof(SkillPage));
            Routing.RegisterRoute(nameof(StatusPage), typeof(StatusPage));
            Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));
        }

        //private async void OnMenuItemClicked(object sender, EventArgs e)
        //{
        //    await Current.GoToAsync("//LoginPage");
        //}
    }
}
