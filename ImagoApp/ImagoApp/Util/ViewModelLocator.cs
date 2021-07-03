using System;
using System.Diagnostics;
using System.Threading;
using AutoMapper;
using ImagoApp.Application.MappingProfiles;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Services;
using ImagoApp.ViewModels;

namespace ImagoApp.Util
{
    public sealed class ViewModelLocator
    {
        private readonly Lazy<IRuleService> _ruleService;
        private readonly Lazy<IWikiService> _wikiService;
        private readonly Lazy<IWikiParseService> _wikiParseService;
        private readonly Lazy<IWikiDataService> _wikiDataService;
        private readonly Lazy<ICharacterService> _characterService;
        private readonly Lazy<ICharacterCreationService> _characterCreationService;

        public ViewModelLocator()
        {
            var databaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Debug.WriteLine("DatabaseFolder: " + databaseFolder);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<WikiDataMappingProfile>();
                cfg.AddProfile<BaseMappingProfile>();
                cfg.AddProfile<CharacterMappingProfile>();
            });

#if DEBUG
            try
            {
                config.AssertConfigurationIsValid();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
#endif

            var mapper = config.CreateMapper();
            
            IWikiDataRepository wikiDataRepository = new WikiDataDataRepository(databaseFolder);
            ICharacterRepository characterRepository = new CharacterRepository(databaseFolder);

            _ruleService = new Lazy<IRuleService>(() => new RuleService());

            _wikiDataService = new Lazy<IWikiDataService>(() => new WikiDataService(wikiDataRepository, mapper));
            _wikiService = new Lazy<IWikiService>(() => new WikiService());
            _wikiParseService = new Lazy<IWikiParseService>(() => new WikiParseService(_wikiDataService.Value));
            _characterService = new Lazy<ICharacterService>(() => new CharacterService(characterRepository, mapper));
            _characterCreationService = new Lazy<ICharacterCreationService>(() => new CharacterCreationService());

            AppShellViewModel = new AppShellViewModel(_characterService.Value);
            AppShellViewModel.EditModeChanged += (sender, value) =>
            {
                App.CurrentCharacterViewModel.EditMode = value;
            };
        }

        public CharacterInfoPageViewModel CharacterInfo => new CharacterInfoPageViewModel(App.CurrentCharacterViewModel, _ruleService.Value);
        public SkillPageViewModel SkillPageViewModel => new SkillPageViewModel(App.CurrentCharacterViewModel, _wikiService.Value, _wikiDataService.Value, _ruleService.Value);
        public StartPageViewModel StartPage => new StartPageViewModel(AppShellViewModel, _characterService.Value, _wikiParseService.Value, _wikiDataService.Value, _ruleService.Value, _characterCreationService.Value);
        public StatusPageViewModel StatusPageViewModel => new StatusPageViewModel(App.CurrentCharacterViewModel, _wikiDataService.Value);
        public InventoryViewModel InventoryViewModel => new InventoryViewModel(App.CurrentCharacterViewModel);
        public AppShellViewModel AppShellViewModel { get; }
        public ChangelogViewModel ChangelogViewModel => new ChangelogViewModel(_wikiService.Value);
        public WikiPageViewModel WikiPageViewModel => new WikiPageViewModel();
    }
}