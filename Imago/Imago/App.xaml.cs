using Imago.Services;
using Imago.Views;
using System;
using System.Diagnostics;
using AutoMapper;
using Imago.Models;
using Imago.Models.Mappings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago
{
    public partial class App : Application
    {
        public  static Character CurrentCharacter { get; set; }
        public static IMapper Mapper;


        public App()
        {
            InitializeComponent();
            var appShell = new AppShell
            {
                //disable flyout to prevent startpage bypassing
                FlyoutBehavior = FlyoutBehavior.Disabled
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TableInfoMapping>();
                cfg.AddProfile<ArmorMapping>();
            });
#if DEBUG
            try
            {
                config.AssertConfigurationIsValid();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
           
#endif

            Mapper = config.CreateMapper();


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
