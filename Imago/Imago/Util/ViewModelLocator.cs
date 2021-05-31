using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Repository;
using Imago.Services;
using Imago.ViewModels;

namespace Imago.Util
{
    public sealed class ViewModelLocator
    {
        private readonly Lazy<ICharacterRepository> _characterRepository;
        private readonly Lazy<ICharacterService> _characterService;

        public ViewModelLocator()
        {
            _characterRepository = new Lazy<ICharacterRepository>(() => new CharacterRepository());
            _characterService = new Lazy<ICharacterService>(() => new CharacterService());
        }
        
        public CharacterInfoPageViewModel CharacterInfo => new CharacterInfoPageViewModel(App.CurrentCharacter, _characterService.Value);
        public SkillPageViewModel SkillPageViewModel => new SkillPageViewModel(App.CurrentCharacter, _characterService.Value);
        public StartPageViewModel StartPage => new StartPageViewModel(_characterRepository.Value);
    }
}
