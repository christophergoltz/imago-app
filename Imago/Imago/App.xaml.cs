using Imago.Services;
using Imago.Views;
using System;
using System.Diagnostics;
using Imago.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago
{
    public partial class App : Application
    {
        public  static Character CurrentCharacter { get; set; }


        public App()
        {
            InitializeComponent();
            var appShell = new AppShell
            {
                //disable flyout to prevent startpage bypassing
                FlyoutBehavior = FlyoutBehavior.Disabled
            };

            MainPage = appShell;
            Shell.Current.GoToAsync($"//{nameof(StartPage)}");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
