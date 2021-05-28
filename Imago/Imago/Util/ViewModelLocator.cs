using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Repository;
using Imago.ViewModels;

namespace Imago.Util
{
    public class ViewModelLocator
    {
        private readonly Lazy<ICharacterRepository> _characterRepository;

        public ViewModelLocator()
        {
            _characterRepository = new Lazy<ICharacterRepository>(() => new CharacterRepository());
        }
        
        public static Character CurrentCharacter { private get; set; }

        public CharacterInfoPageViewModel CharacterInfo => new CharacterInfoPageViewModel(CurrentCharacter);
        public StartPageViewModel StartPage => new StartPageViewModel(_characterRepository.Value);
    }
}
