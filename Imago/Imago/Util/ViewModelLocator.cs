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
        private readonly Lazy<IRuleRepository> _ruleRepository;

        public ViewModelLocator()
        {
            _characterRepository = new Lazy<ICharacterRepository>(() => new CharacterRepository());
            _ruleRepository = new Lazy<IRuleRepository>(() => new RuleRepository());
            _characterService = new Lazy<ICharacterService>(() => new CharacterService(_ruleRepository.Value));
        }
        
        public CharacterInfoPageViewModel CharacterInfo => new CharacterInfoPageViewModel(App.CurrentCharacter, _characterService.Value, _ruleRepository.Value);
        public SkillPageViewModel SkillPageViewModel => new SkillPageViewModel(App.CurrentCharacter, _characterService.Value);
        public StartPageViewModel StartPage => new StartPageViewModel(_characterRepository.Value);
        public StatusPageViewModel StatusPageViewModel => new StatusPageViewModel(App.CurrentCharacter);
        public InventoryViewModel InventoryViewModel => new InventoryViewModel(App.CurrentCharacter);
        public AppShellViewModel AppShellViewModel => new AppShellViewModel();
    }
}
