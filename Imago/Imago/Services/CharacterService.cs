using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Imago.Models;
using Imago.Repository.WrappingDatabase;
using Imago.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Imago.Services
{
    public interface ICharacterService
    {
        void SetCurrentCharacter(CharacterViewModel character);
        CharacterViewModel GetCurrentCharacter();
        Task<bool> SaveCurrentCharacter();
    }

    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        private CharacterViewModel _currentCharacter;

        public void SetCurrentCharacter(CharacterViewModel character)
        {
            _currentCharacter = character;
        }

        public async Task<bool> SaveCurrentCharacter()
        {
            Debug.WriteLine("Start saving..");
            var result = await _characterRepository.Update(_currentCharacter.Character);
            Debug.WriteLine("Done saving..");

            return result;
        }
        
        public CharacterViewModel GetCurrentCharacter()
        {
            return _currentCharacter;
        }
    }
}