using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Entity;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.ViewModels;

namespace Imago.Util
{
    public sealed class ViewModelLocator
    {
        private readonly Lazy<ICharacterRepository> _characterRepository;
        private readonly Lazy<ICharacterService> _characterService;
        private readonly Lazy<IRuleRepository> _ruleRepository;
        private readonly Lazy<IChangeLogRepository> _changeLogRepository;
        private readonly Lazy<IWikiRepository> _wikiRepository;
        private readonly Lazy<IItemRepository> _itemRepository;
        private readonly Lazy<IWikiParseService> _wikiParseService;

        private readonly Lazy<IWrappingRepository<Weapon, WeaponEntity>> _meleeWeaponRepository;
        private readonly Lazy<IWrappingRepository<Weapon, WeaponEntity>> _rangedWeaponRepository;



        public ViewModelLocator()
        {
            var databaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            _meleeWeaponRepository = new Lazy<IWrappingRepository<Weapon, WeaponEntity>>(() => MeleeWeaponRepository.Setup(databaseFolder).Result);
            _rangedWeaponRepository = new Lazy<IWrappingRepository<Weapon, WeaponEntity>>(() => MeleeWeaponRepository.Setup(databaseFolder).Result);

            _characterRepository = new Lazy<ICharacterRepository>(() => new CharacterRepository());
            _ruleRepository = new Lazy<IRuleRepository>(() => new RuleRepository());
            _wikiRepository = new Lazy<IWikiRepository>(() => new WikiRepository());
            _changeLogRepository = new Lazy<IChangeLogRepository>(() => new ChangeLogRepository());
            _characterService = new Lazy<ICharacterService>(() => new CharacterService(_ruleRepository.Value));
            _itemRepository = new Lazy<IItemRepository>(() => new ItemRepository());
            _wikiParseService = new Lazy<IWikiParseService>(() => new WikiParseService(_meleeWeaponRepository.Value, _rangedWeaponRepository.Value));

           

          
        }
        
        public CharacterInfoPageViewModel CharacterInfo => new CharacterInfoPageViewModel(App.CurrentCharacter, _characterService.Value, _ruleRepository.Value);
        public SkillPageViewModel SkillPageViewModel => new SkillPageViewModel(App.CurrentCharacter, _characterService.Value, _wikiRepository.Value);
        public StartPageViewModel StartPage => new StartPageViewModel(_characterRepository.Value,_wikiParseService.Value);
        public StatusPageViewModel StatusPageViewModel => new StatusPageViewModel(App.CurrentCharacter, _itemRepository.Value, _characterService.Value);
        public InventoryViewModel InventoryViewModel => new InventoryViewModel(App.CurrentCharacter, _characterService.Value);
        public AppShellViewModel AppShellViewModel => new AppShellViewModel();
        public ChangelogViewModel ChangelogViewModel => new ChangelogViewModel(_changeLogRepository.Value);
        public WikiPageViewModel WikiPageViewModel => new WikiPageViewModel();
    }
}
