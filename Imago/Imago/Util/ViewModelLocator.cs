using System;
using System.Collections.Generic;
using System.Text;
using Imago.Database;
using Imago.Models;
using Imago.Models.Entity;
using Imago.Repository;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Imago.Util
{
    public sealed class ViewModelLocator
    {
        private readonly Lazy<ICharacterRepository> _characterRepository;
        private readonly Lazy<ICharacterService> _characterService;
        private readonly Lazy<IRuleRepository> _ruleRepository;
        private readonly Lazy<IChangeLogRepository> _changeLogRepository;
        private readonly Lazy<IWikiRepository> _wikiRepository;
        private readonly Lazy<IWikiParseService> _wikiParseService;

        private readonly IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly IRangedWeaponRepository _rangedWeaponRepository;
        private readonly IArmorRepository _armorRepository;
        
        public ViewModelLocator()
        {
            var databaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            _meleeWeaponRepository = new MeleeWeaponRepository(databaseFolder);
            _rangedWeaponRepository = new RangedWeaponRepository(databaseFolder);
            _armorRepository = new ArmorRepository(databaseFolder);

            _characterRepository = new Lazy<ICharacterRepository>(() => new CharacterRepository());
            _ruleRepository = new Lazy<IRuleRepository>(() => new RuleRepository());
            _wikiRepository = new Lazy<IWikiRepository>(() => new WikiRepository());
            _changeLogRepository = new Lazy<IChangeLogRepository>(() => new ChangeLogRepository());
            _characterService = new Lazy<ICharacterService>(() => new CharacterService(_ruleRepository.Value));
            _wikiParseService = new Lazy<IWikiParseService>(() => new WikiParseService(_meleeWeaponRepository, _rangedWeaponRepository, _armorRepository));

           

          
        }
        
        public CharacterInfoPageViewModel CharacterInfo => new CharacterInfoPageViewModel(App.CurrentCharacter, _characterService.Value, _ruleRepository.Value);
        public SkillPageViewModel SkillPageViewModel => new SkillPageViewModel(App.CurrentCharacter, _characterService.Value, _wikiRepository.Value);
        public StartPageViewModel StartPage => new StartPageViewModel(_characterRepository.Value,_wikiParseService.Value,_meleeWeaponRepository, _rangedWeaponRepository, _armorRepository);
        public StatusPageViewModel StatusPageViewModel => new StatusPageViewModel(App.CurrentCharacter,_armorRepository, _meleeWeaponRepository, _rangedWeaponRepository, _characterService.Value);
        public InventoryViewModel InventoryViewModel => new InventoryViewModel(App.CurrentCharacter, _characterService.Value);
        public AppShellViewModel AppShellViewModel => new AppShellViewModel();
        public ChangelogViewModel ChangelogViewModel => new ChangelogViewModel(_changeLogRepository.Value);
        public WikiPageViewModel WikiPageViewModel => new WikiPageViewModel();
    }
}
