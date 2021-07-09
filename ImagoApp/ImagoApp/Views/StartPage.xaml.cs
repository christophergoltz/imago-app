using System;
using System.Linq;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage(StartPageViewModel startPageViewModel)
        {
            BindingContext = startPageViewModel;
            InitializeComponent();
        }
    }
}