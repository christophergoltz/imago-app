using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imago.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterInfoPage : ContentPage
    {
        public CharacterInfoPage()
        {
            InitializeComponent();
        }

        private void CharacterInfoPage_OnAppearing(object sender, EventArgs e)
        {
            ((CharacterInfoPageViewModel) BindingContext).OpenAttributeExperienceDialogIfNeeded();
        }
    }
}