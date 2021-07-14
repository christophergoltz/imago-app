using System;
using ImagoApp.Infrastructure.Repositories;
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

        private static ServiceLocator _serviceLocator;

        public App(IFileService fileService)
        {
            InitializeComponent();

            var localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _serviceLocator = new ServiceLocator(localApplicationData);

#if DEBUG
            AppCenter.Start("uwp=4350071e-000b-4ab6-bfae-369afc829008;", typeof(Analytics), typeof(Crashes));
#elif RELEASE
            AppCenter.Start("uwp=5b35b16b-6bde-4772-9972-b7d1809327fb;",typeof(Analytics), typeof(Crashes));
#endif
            
            var startPageViewModel = new StartPageViewModel(_serviceLocator, _serviceLocator.CharacterService(),
                _serviceLocator.WikiParseService(), _serviceLocator.WikiDataService(),
                _serviceLocator.RuleService(),
                _serviceLocator.CharacterCreationService(), _serviceLocator.WikiService(),
                localApplicationData, fileService);

            MainPage = new StartPage(startPageViewModel);
        }

        public static bool SaveCurrentCharacter()
        {
            if (CurrentCharacterViewModel == null)
                return true;

            return _serviceLocator.CharacterService().SaveCharacter(CurrentCharacterViewModel.CharacterModel);
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
