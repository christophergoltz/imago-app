using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DicePage : ContentPage
    {
        public DicePageViewModel DicePageViewModel { get; }

        public DicePage(DicePageViewModel dicePageViewModel)
        {
            BindingContext = DicePageViewModel = dicePageViewModel;
            InitializeComponent();
        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            //if (e.Url != DicePageViewModel.SkillWikiSource.BaseUrl)
            //    e.Cancel = true;
        }
    }
}