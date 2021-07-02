using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
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
            ((ViewModels.CharacterInfoPageViewModel) BindingContext).OpenAttributeExperienceDialogIfNeeded();
        }
    }
}