using System;
using System.Diagnostics;
using System.Threading;
using AutoMapper;
using ImagoApp.Application.MappingProfiles;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Repositories;
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
        private readonly IMapper _mapper;
       
        public ViewModelLocator(string imagoFolder)
        {
            Debug.WriteLine("DatabaseFolder: " + imagoFolder);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<WikiDataMappingProfile>();
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

            _mapper = config.CreateMapper();
            
            ICharacterRepository characterRepository = new CharacterRepository(imagoFolder);
            IArmorTemplateRepository armorTemplateRepository = new ArmorTemplateRepository(imagoFolder);
            IWeaponTemplateRepository weaponTemplateRepository = new WeaponTemplateRepository(imagoFolder);
            ITalentRepository talentRepository = new TalentRepository(imagoFolder);
            IMasteryRepository masteryRepository = new MasteryRepository(imagoFolder);

            _ruleService = new Lazy<IRuleService>(() => new RuleService());
            _wikiDataService = new Lazy<IWikiDataService>(() => new WikiDataService(_mapper, armorTemplateRepository, 
                weaponTemplateRepository, talentRepository, masteryRepository));
            _wikiService = new Lazy<IWikiService>(() => new WikiService());
            _wikiParseService = new Lazy<IWikiParseService>(() => new WikiParseService(_wikiDataService.Value));
            _characterService = new Lazy<ICharacterService>(() => new CharacterService(characterRepository, _mapper));
            _characterCreationService = new Lazy<ICharacterCreationService>(() => new CharacterCreationService());
        }

        public IMapper Mapper()
        {
            return _mapper;
        }

        public IRuleService RuleService()
        {
            return _ruleService.Value;
        }

        public IWikiDataService WikiDataService()
        {
            return _wikiDataService.Value;
        }
        
        public IWikiService WikiService()
        {
            return _wikiService.Value;
        }
        public IWikiParseService WikiParseService()
        {
            return _wikiParseService.Value;
        }
        public ICharacterService CharacterService()
        {
            return _characterService.Value;
        }
        public ICharacterCreationService CharacterCreationService()
        {
            return _characterCreationService.Value;
        }
    }
}