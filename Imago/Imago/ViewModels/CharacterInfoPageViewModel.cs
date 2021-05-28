using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;

namespace Imago.ViewModels
{
    public class CharacterInfoPageViewModel : BaseViewModel
    {
        public CharacterInfoPageViewModel(Character character)
        {
            Title = character.Name;
            Character = character;
        }

        public Character Character { get; set; }
    }
}
