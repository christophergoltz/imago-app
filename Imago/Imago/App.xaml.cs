using Imago.Services;
using Imago.Views;
using System;
using System.Diagnostics;
using Imago.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
            
#if DEBUG
            AppCenter.Start("uwp=4350071e-000b-4ab6-bfae-369afc829008;", typeof(Analytics), typeof(Crashes));
#elif RELEASE
            AppCenter.Start("uwp=5b35b16b-6bde-4772-9972-b7d1809327fb;",typeof(Analytics), typeof(Crashes));
#endif


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
