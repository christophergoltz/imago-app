using System;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ImagoApp.Application.Models;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPageViewModel StartPageViewModel { get; }

        public StartPage(StartPageViewModel startPageViewModel)
        {
            BindingContext = StartPageViewModel = startPageViewModel;
            InitializeComponent();
        }

        private void CharacterListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var character = (CharacterModel) e.SelectedItem;

            //reset listview selection
            var listView = (ListView) sender;
            listView.SelectedItem = null;

            Task.Run(async () =>
            {
                try
                {
                    using (UserDialogs.Instance.Loading("Character wird geladen.."))
                    {
                        await Task.Delay(250);
                        await StartPageViewModel.OpenCharacter(character, false);
                        await Task.Delay(250);
                    }
                }
                catch (Exception exception)
                {
                    App.ErrorManager.TrackException(exception, character.Name);
                }
            });
        }
    }
}