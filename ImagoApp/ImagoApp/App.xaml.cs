using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AutoMapper;
using DryIoc;
using ImagoApp.Application.MappingProfiles;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Database;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Manager;
using ImagoApp.ViewModels;
using ImagoApp.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace ImagoApp
{
    public partial class App
    {
        public static ErrorManager ErrorManager;
        public static StartPage StartPage;

        public static Container Container;

        public App(ILocalFileService localFileService)
        {
            InitializeComponent();

            //configure appcenter
#if DEBUG
            AppCenter.Start("uwp=4350071e-000b-4ab6-bfae-369afc829008;", typeof(Analytics), typeof(Crashes));
#elif RELEASE
            AppCenter.Start("uwp=5b35b16b-6bde-4772-9972-b7d1809327fb;",typeof(Analytics), typeof(Crashes));
#endif
            AppCenter.SetUserId(DeviceInfo.Name);
            Crashes.SetEnabledAsync(true);

            CreateContainer(localFileService);

            ErrorManager = Container.Resolve<ErrorManager>();
            
            MainPage = new StartPage(Container.Resolve<StartPageViewModel>());
        }

        private void CreateContainer(ILocalFileService localFileService)
        {
            var imagoFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var wikidataDatabaseFile = Path.Combine(imagoFolder, "ImagoApp_Wikidata.db");

            var container = new Container();

            //localfileservice
            container.RegisterInstance(localFileService);

            //mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<WikiDataMappingProfile>();
                cfg.AddProfile<CharacterMappingProfile>();
            });

#if DEBUG
            try
            {
                config.AssertConfigurationIsValid();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
#endif

            var mapper = config.CreateMapper();

            container.RegisterInstance(mapper);

            //repositories
            container.RegisterInstance<IArmorTemplateRepository>(new ArmorTemplateRepository(wikidataDatabaseFile));
            container.RegisterInstance<IWeaponTemplateRepository>(new WeaponTemplateRepository(wikidataDatabaseFile));
            container.RegisterInstance<ITalentRepository>(new TalentRepository(wikidataDatabaseFile));
            container.RegisterInstance<IMasteryRepository>(new MasteryRepository(wikidataDatabaseFile));
            container.RegisterInstance<IWeaveTalentRepository>(new WeaveTalentRepository(wikidataDatabaseFile));

            //services
            container.Register<IFileService, FileService>();
            container.Register<IWikiDataService, WikiDataService>();
            container.Register<IWikiService, WikiService>();
            container.Register<IWikiParseService, WikiParseService>();
            container.Register<ICharacterCreationService, CharacterCreationService>();
            container.Register<IIncreaseCalculationService, IncreaseCalculationService>();
            container.Register<ISkillCalculationService, SkillCalculationService>();
            container.Register<ISkillGroupCalculationService, SkillGroupCalculationService>();
            container.Register<IAttributeCalculationService, AttributeCalculationService>();

            //character
            container.Register<ICharacterProvider, CharacterProvider>(Reuse.Singleton);

            var databaseFolder = container.Resolve<IFileService>().GetCharacterDatabaseFolder();
            Debug.WriteLine($"DatabaseFolder: {databaseFolder}");

            container.RegisterInstance<ICharacterDatabaseConnection>(new CharacterDatabaseConnection(databaseFolder));

            string DatabaseFile()
            {
                var characterModelGuid = container.Resolve<ICharacterProvider>().CurrentCharacter.CharacterModel.Guid;
                return container.Resolve<ICharacterDatabaseConnection>().GetCharacterDatabaseFile(characterModelGuid);
            }

            container.RegisterInstance<ICharacterRepository>(new CharacterRepository(DatabaseFile,
                container.Resolve<ICharacterDatabaseConnection>()));
            container.Register<ICharacterService, CharacterService>();
            container.RegisterDelegate(() => container.Resolve<ICharacterProvider>().CurrentCharacter);

            //error
            container.Register<IErrorService, ErrorService>();
            container.Register<ErrorManager>();

            //viewmodel
            container.Register<StartPageViewModel>();

            Container = container;
        }

        public static bool SaveCurrentCharacter()
        {
            var characterViewModel = Container.Resolve<ICharacterProvider>().CurrentCharacter;
            if (characterViewModel == null)
                return true;

            return Container.Resolve<ICharacterService>().SaveCharacter(characterViewModel.CharacterModel);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            try
            {
                var result = SaveCurrentCharacter();
                if (!result)
                {
                    ErrorManager.TrackExceptionSilent(
                        new InvalidOperationException("Character coulnd be saved.. unknown reason"),
                        new Dictionary<string, string>()
                        {
                            {"SaveResult", "false"}
                        });
                }

            }
            catch (Exception e)
            {
                ErrorManager.TrackExceptionSilent(e,
                    new Dictionary<string, string>()
                    {
                        {"Event", "OnSleep;Saving Character"}
                    });
            }
        }

        protected override void OnResume()
        {
        }
    }
}