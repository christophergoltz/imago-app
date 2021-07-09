using System;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterInfoPage : ContentPage
    {
        public CharacterInfoPageViewModel CharacterInfoPageViewModel { get; }
        public CharacterInfoPage(CharacterInfoPageViewModel characterInfoPageViewModel)
        {
            BindingContext = CharacterInfoPageViewModel = characterInfoPageViewModel;
            InitializeComponent();
        }

        private void CharacterInfoPage_OnAppearing(object sender, EventArgs e)
        {
            CharacterInfoPageViewModel.OpenAttributeExperienceDialogIfNeeded();
        }
    }
}