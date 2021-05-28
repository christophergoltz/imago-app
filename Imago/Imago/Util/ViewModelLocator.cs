using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Repository;
using Imago.Services;
using Imago.ViewModels;

namespace Imago.Util
{
    public class ViewModelLocator
    {
        private readonly Lazy<ICharacterRepository> _characterRepository;
        private readonly Lazy<IAttributeService> _attributeService;

        public ViewModelLocator()
        {
            _characterRepository = new Lazy<ICharacterRepository>(() => new CharacterRepository());
            _attributeService = new Lazy<IAttributeService>(() => new AttributeService());
        }
        
        public static Character CurrentCharacter { private get; set; }

        public CharacterInfoPageViewModel CharacterInfo => new CharacterInfoPageViewModel(CurrentCharacter, _attributeService.Value);
        public SkillPageViewModel SkillPageViewModel => new SkillPageViewModel(CurrentCharacter);
        public StartPageViewModel StartPage => new StartPageViewModel(_characterRepository.Value);
    }
}
