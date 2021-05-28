using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Util;
using Imago.Views;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private readonly ICharacterRepository _characterRepository;

        public StartPageViewModel(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
            TestCharacterCommand = new Command(OnTestCharacterClicked);
        }

        public Command TestCharacterCommand { get; }

        private async void OnTestCharacterClicked(object obj)
        {
            var newChar = _characterRepository.CreateNewCharacter();

            newChar.Name = "Testspieler";
            newChar.RaceType = RaceType.Mensch;
            newChar.CreatedBy = "System";
            newChar.Owner = "Testuser";

            //unlock flyout
            if (Application.Current.MainPage is AppShell app)
                app.FlyoutBehavior = FlyoutBehavior.Locked;

            ViewModelLocator.CurrentCharacter = newChar;
            await Shell.Current.GoToAsync($"//{nameof(CharacterInfoPage)}");
        }

    }
}
