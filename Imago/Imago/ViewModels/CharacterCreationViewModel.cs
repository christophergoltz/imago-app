using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Repository;
using Imago.Util;

namespace Imago.ViewModels
{
    public class CharacterCreationViewModel : BindableBase
    {
        public Character Character { get; set; }

        public CharacterCreationViewModel(ICharacterRepository characterRepository)
        {
            Character = characterRepository.CreateNewCharacter();
        }
    }
}
