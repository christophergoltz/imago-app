using System;
using System.Diagnostics;

namespace ImagoApp.Util
{
    public sealed class ViewModelLocator
    {
        private readonly Lazy<Repository.IRuleRepository> _ruleRepository;
        private readonly Lazy<Services.IWikiService> _wikiService;
        private readonly Lazy<Services.IWikiParseService> _wikiParseService;
        private readonly Lazy<Services.ICharacterService> _characterService;

        private readonly Repository.WrappingDatabase.IMeleeWeaponRepository _meleeWeaponRepository;
        private readonly Repository.WrappingDatabase.IRangedWeaponRepository _rangedWeaponRepository;
        private readonly Repository.WrappingDatabase.IArmorRepository _armorRepository;
        private readonly Repository.WrappingDatabase.ITalentRepository _talentRepository;
        private readonly Repository.WrappingDatabase.IMasteryRepository _masteryRepository;
        private readonly Repository.WrappingDatabase.ISpecialWeaponRepository _specialWeaponRepository;
        private readonly Repository.WrappingDatabase.IShieldRepository _shieldRepository;
        private readonly Repository.WrappingDatabase.ICharacterRepository _characterRepository;

        public ViewModelLocator()
        {
            var databaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Debug.WriteLine("DatabaseFolder: " + databaseFolder);

            _meleeWeaponRepository = new Repository.WrappingDatabase.MeleeWeaponRepository(databaseFolder);
            _rangedWeaponRepository = new Repository.WrappingDatabase.RangedWeaponRepository(databaseFolder);
            _armorRepository = new Repository.WrappingDatabase.ArmorRepository(databaseFolder);
            _talentRepository = new Repository.WrappingDatabase.TalentRepository(databaseFolder);
            _specialWeaponRepository = new Repository.WrappingDatabase.SpecialWeaponRepository(databaseFolder);
            _shieldRepository = new Repository.WrappingDatabase.ShieldRepository(databaseFolder);
            _masteryRepository = new Repository.WrappingDatabase.MasteryRepository(databaseFolder);
            _characterRepository = new Repository.WrappingDatabase.CharacterRepository(databaseFolder);

            _ruleRepository = new Lazy<Repository.IRuleRepository>(() => new Repository.RuleRepository());
            _wikiService = new Lazy<Services.IWikiService>(() => new Services.WikiService());
            _wikiParseService = new Lazy<Services.IWikiParseService>(() => new Services.WikiParseService(_meleeWeaponRepository,
                _rangedWeaponRepository,
                _armorRepository, _talentRepository, _specialWeaponRepository, _shieldRepository, _masteryRepository));
            _characterService = new Lazy<Services.ICharacterService>(() => new Services.CharacterService(_characterRepository));

            AppShellViewModel = new ViewModels.AppShellViewModel(_characterService.Value);
            AppShellViewModel.EditModeChanged += (sender, value) =>
            {
                _characterService.Value.GetCurrentCharacter().EditMode = value;
            };
        }

        public ViewModels.CharacterInfoPageViewModel CharacterInfo => new ViewModels.CharacterInfoPageViewModel(_characterService.Value.GetCurrentCharacter(), _ruleRepository.Value);

        public ViewModels.SkillPageViewModel SkillPageViewModel => new ViewModels.SkillPageViewModel(_characterService.Value.GetCurrentCharacter(), _wikiService.Value,
            _masteryRepository, _talentRepository, _ruleRepository.Value);

        public ViewModels.StartPageViewModel StartPage => new ViewModels.StartPageViewModel(AppShellViewModel, _characterRepository, _characterService.Value, _wikiParseService.Value,
            _meleeWeaponRepository, _rangedWeaponRepository,
            _armorRepository, _talentRepository, _specialWeaponRepository, _shieldRepository, _masteryRepository,
            _ruleRepository.Value);

        public ViewModels.StatusPageViewModel StatusPageViewModel => new ViewModels.StatusPageViewModel(_characterService.Value.GetCurrentCharacter(),
            _armorRepository, _meleeWeaponRepository, _rangedWeaponRepository, _specialWeaponRepository,
            _shieldRepository);

        public ViewModels.InventoryViewModel InventoryViewModel => new ViewModels.InventoryViewModel(_characterService.Value.GetCurrentCharacter());
        public ViewModels.AppShellViewModel AppShellViewModel { get; }
        public ViewModels.ChangelogViewModel ChangelogViewModel => new ViewModels.ChangelogViewModel(_wikiService.Value);
        public ViewModels.WikiPageViewModel WikiPageViewModel => new ViewModels.WikiPageViewModel();
    }
}