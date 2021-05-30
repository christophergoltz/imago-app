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
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//LoginPage");
        }
    }
}
