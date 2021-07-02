using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace ImagoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
#if DEBUG
            AppCenter.Start("uwp=4350071e-000b-4ab6-bfae-369afc829008;", typeof(Analytics), typeof(Crashes));
#elif RELEASE
            AppCenter.Start("uwp=5b35b16b-6bde-4772-9972-b7d1809327fb;",typeof(Analytics), typeof(Crashes));
#endif

            MainPage = new Views.StartPage();
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
