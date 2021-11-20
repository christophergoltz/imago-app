using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ImagoApp.Application.Models;
using ImagoApp.Shared;
using ImagoApp.ViewModels;
using Microsoft.AppCenter.Analytics;
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

        private void OpenSelectedCharacter(object sender, EventArgs e)
        {
            var listview = (StackLayout) sender;
            var character = (CharacterPreview)listview.BindingContext;

            Task.Run(async () =>
            {
                try
                {
                    using (UserDialogs.Instance.Loading("Character wird geladen.."))
                    {
                        await Task.Delay(250);

                        Analytics.TrackEvent("Open Character", new Dictionary<string, string>
                        {
                            {
                                "Name", character.Name
                            }
                        });

                        var characterModel = StartPageViewModel.GetCharacter(character);
                        await StartPageViewModel.OpenCharacter(characterModel, false);
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