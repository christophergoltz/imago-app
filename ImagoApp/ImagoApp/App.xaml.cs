using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Autofac;
using Autofac.Core;
using AutoMapper;
using ImagoApp.Application.MappingProfiles;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Database;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Manager;
using ImagoApp.Styles;
using ImagoApp.Styles.Themes;
using ImagoApp.ViewModels;
using ImagoApp.Views;
using ImagoApp.Views.CustomControls;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace ImagoApp
{
    public partial class App
    {
        public static ErrorManager ErrorManager;
        public static StartPage StartPage;

        public static IContainer Container;

        public static string TempFolder;

        public App(ILocalFileService localFileService)
        {
            InitializeComponent();

            Sharpnado.Tabs.Initializer.Initialize(false, false);
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);

            //configure appcenter
#if DEBUG
            AppCenter.Start("uwp=4350071e-000b-4ab6-bfae-369afc829008;", typeof(Analytics), typeof(Crashes));
#elif RELEASE
            AppCenter.Start("uwp=5b35b16b-6bde-4772-9972-b7d1809327fb;",typeof(Analytics), typeof(Crashes));
#endif
            AppCenter.SetUserId(DeviceInfo.Name);
            Crashes.SetEnabledAsync(true);

            Analytics.TrackEvent("Startup");
            
            CreateContainer(localFileService);

            ErrorManager = Container.Resolve<ErrorManager>();

            MainPage = new StartPage(Container.Resolve<StartPageViewModel>());
        }
        
        private IMapper CreateMapper()
        {
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

            return config.CreateMapper();
        }

        private void CreateContainer(ILocalFileService localFileService)
        {
            var applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var wikiDatabaseFile = Path.Combine(applicationDataFolder, "ImagoApp_Wikidata.db");

            TempFolder = Path.Combine(applicationDataFolder, "Temp");

            var builder = new ContainerBuilder();

            //localfileservice
            builder.RegisterInstance(localFileService);

            //mapper
            builder.RegisterInstance(CreateMapper());

            //repositories
            builder.RegisterInstance<IArmorTemplateRepository>(new ArmorTemplateRepository(wikiDatabaseFile));
            builder.RegisterInstance<IWeaponTemplateRepository>(new WeaponTemplateRepository(wikiDatabaseFile));
            builder.RegisterInstance<ITalentRepository>(new TalentRepository(wikiDatabaseFile));
            builder.RegisterInstance<IMasteryRepository>(new MasteryRepository(wikiDatabaseFile));
            builder.RegisterInstance<IWeaveTalentRepository>(new WeaveTalentRepository(wikiDatabaseFile));

            //services
            builder.RegisterType<FileRepository>().As<IFileRepository>();
            builder.RegisterType<WikiDataService>().As<IWikiDataService>();
            builder.RegisterType<WikiService>().As<IWikiService>();
            builder.RegisterType<WikiParseService>().As<IWikiParseService>();
            builder.RegisterType<CharacterCreationService>().As<ICharacterCreationService>();
            builder.RegisterType<SkillCalculationService>().As<ISkillCalculationService>();
            builder.RegisterType<SkillGroupCalculationService>().As<ISkillGroupCalculationService>();
            builder.RegisterType<AttributeCalculationService>().As<IAttributeCalculationService>();

            //character
            builder.RegisterType<CharacterProvider>().As<ICharacterProvider>().SingleInstance();
            builder.RegisterType<CharacterDatabaseConnection>().As<ICharacterDatabaseConnection>().SingleInstance();
            builder.RegisterType<CharacterRepository>().As<ICharacterRepository>().SingleInstance();
            builder.RegisterType<CharacterService>().As<ICharacterService>();

            //error
            builder.RegisterType<ErrorService>().As<IErrorService>();
            builder.RegisterType<ErrorManager>();

            //viewmodel
            builder.RegisterType<StartPageViewModel>();
            builder.Register<CharacterViewModel>(context =>
            {
                var characterProvider = context.Resolve<ICharacterProvider>();
                return characterProvider.CurrentCharacter;
            });

            Container = builder.Build();
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

        public static object GetAppResourcesByName(string name)
        {
            if (Current.Resources.TryGetValue(name, out var result))
                return result;

            foreach (var mergedDictionary in Current.Resources.MergedDictionaries)
            {
                if (mergedDictionary.TryGetValue(name, out result))
                    return result;
            }

            throw new KeyNotFoundException(name);
        }
    }
}