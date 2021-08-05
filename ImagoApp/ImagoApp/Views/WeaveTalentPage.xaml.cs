using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeaveTalentPage : ContentPage
    {
        public WeaveTalentPageViewModel ViewModel { get; set; }
        public WeaveTalentPage(WeaveTalentPageViewModel weaveTalentPageViewModel)
        {
            BindingContext = ViewModel = weaveTalentPageViewModel;
            InitializeComponent();
        }
    }
}