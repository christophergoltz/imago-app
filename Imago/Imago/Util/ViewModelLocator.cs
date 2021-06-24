using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly Lazy<IRuleRepository> _ruleRepository;
        private readonly Lazy<IWikiService> _wikiService;
        private readonly Lazy<IWikiParseService> _wikiParseService;

        private readonly IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly IRangedWeaponRepository _rangedWeaponRepository;
        private readonly IArmorRepository _armorRepository;
        private readonly ITalentRepository _talentRepository;
        private readonly IMasteryRepository _masteryRepository;
        private readonly ISpecialWeaponRepository _specialWeaponRepository;
        private readonly IShieldRepository _shieldRepository;
        private readonly ICharacterRepository _characterRepository;

        public ViewModelLocator()
        {
            var databaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Debug.WriteLine("DatabaseFolder: " + databaseFolder);

            _meleeWeaponRepository = new MeleeWeaponRepository(databaseFolder);
            _rangedWeaponRepository = new RangedWeaponRepository(databaseFolder);
            _armorRepository = new ArmorRepository(databaseFolder);
            _talentRepository = new TalentRepository(databaseFolder);
            _specialWeaponRepository = new SpecialWeaponRepository(databaseFolder);
            _shieldRepository = new ShieldRepository(databaseFolder);
            _masteryRepository = new MasteryRepository(databaseFolder);
            _characterRepository = new CharacterRepository(databaseFolder);

            _ruleRepository = new Lazy<IRuleRepository>(() => new RuleRepository());
            _wikiService = new Lazy<IWikiService>(() => new WikiService());
            _wikiParseService = new Lazy<IWikiParseService>(() => new WikiParseService(_meleeWeaponRepository,
                _rangedWeaponRepository,
                _armorRepository, _talentRepository, _specialWeaponRepository, _shieldRepository, _masteryRepository));

            AppShellViewModel = new AppShellViewModel();
            AppShellViewModel.EditModeChanged += (sender, value) => { App.CurrentCharacter.EditMode = value; };
        }

        public CharacterInfoPageViewModel CharacterInfo =>
            new CharacterInfoPageViewModel(App.CurrentCharacter, _ruleRepository.Value);

        public SkillPageViewModel SkillPageViewModel => new SkillPageViewModel(App.CurrentCharacter, _wikiService.Value,
            _masteryRepository, _talentRepository, _ruleRepository.Value);

        public StartPageViewModel StartPage => new StartPageViewModel(_characterRepository, _wikiParseService.Value,
            _meleeWeaponRepository, _rangedWeaponRepository,
            _armorRepository, _talentRepository, _specialWeaponRepository, _shieldRepository, _masteryRepository,
            _ruleRepository.Value);

        public StatusPageViewModel StatusPageViewModel => new StatusPageViewModel(App.CurrentCharacter,
            _armorRepository, _meleeWeaponRepository, _rangedWeaponRepository, _specialWeaponRepository,
            _shieldRepository);

        public InventoryViewModel InventoryViewModel => new InventoryViewModel(App.CurrentCharacter);
        public AppShellViewModel AppShellViewModel { get; }
        public ChangelogViewModel ChangelogViewModel => new ChangelogViewModel(_wikiService.Value);
        public WikiPageViewModel WikiPageViewModel => new WikiPageViewModel();
    }
}