using Imago.Services;
using Imago.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
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
