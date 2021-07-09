using System;
using ImagoApp.Util;
using ImagoApp.ViewModels;
using ImagoApp.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace ImagoApp
{
    public partial class App : Xamarin.Forms.Application
    {
        public static StartPage StartPage;
        public static CharacterViewModel CurrentCharacterViewModel { get; set; }

        public App()
        {
            InitializeComponent();

            var viewModelLocator = new ViewModelLocator();
            var localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
#if DEBUG
            AppCenter.Start("uwp=4350071e-000b-4ab6-bfae-369afc829008;", typeof(Analytics), typeof(Crashes));
#elif RELEASE
            AppCenter.Start("uwp=5b35b16b-6bde-4772-9972-b7d1809327fb;",typeof(Analytics), typeof(Crashes));
#endif

            var startPageViewModel = new StartPageViewModel(viewModelLocator, viewModelLocator.CharacterService(),
                viewModelLocator.WikiParseService(), viewModelLocator.WikiDataService(), viewModelLocator.RuleService(),
                viewModelLocator.CharacterCreationService(), viewModelLocator.WikiService(), localApplicationData);

            MainPage = new StartPage(startPageViewModel);
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
