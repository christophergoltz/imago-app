﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AutoMapper;
using ImagoApp.Application.MappingProfiles;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.ViewModels;

namespace ImagoApp.Util
{
    public sealed class ServiceLocator
    {
        private readonly Lazy<IWikiService> _wikiService;
        private readonly Lazy<IWikiParseService> _wikiParseService;
        private readonly Lazy<IWikiDataService> _wikiDataService;
        private readonly Lazy<ICharacterService> _characterService;
        private readonly Lazy<ICharacterCreationService> _characterCreationService;
        private readonly Lazy<IErrorService> _errorService;
        private readonly Lazy<IAttributeCalculationService> _attributeCalculationService;
        private readonly Lazy<ISkillGroupCalculationService> _skillGroupCalculationService;
        private readonly Lazy<ISkillCalculationService> _skillCalculationService;
        private readonly Lazy<IIncreaseCalculationService> _increaseCalculationService;
        private readonly IMapper _mapper;
       
        public ServiceLocator(string imagoFolder)
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
            
            var characterDatabaseFile = Path.Combine(imagoFolder, "ImagoApp_Character.db");
            var wikidataDatabaseFile = Path.Combine(imagoFolder, "ImagoApp_Wikidata.db");

            ICharacterRepository characterRepository = new CharacterRepository(characterDatabaseFile);
            IArmorTemplateRepository armorTemplateRepository = new ArmorTemplateRepository(wikidataDatabaseFile);
            IWeaponTemplateRepository weaponTemplateRepository = new WeaponTemplateRepository(wikidataDatabaseFile);
            ITalentRepository talentRepository = new TalentRepository(wikidataDatabaseFile);
            IMasteryRepository masteryRepository = new MasteryRepository(wikidataDatabaseFile);
            
            _wikiDataService = new Lazy<IWikiDataService>(() => new WikiDataService(_mapper, armorTemplateRepository, 
                weaponTemplateRepository, talentRepository, masteryRepository));
            _wikiService = new Lazy<IWikiService>(() => new WikiService());
            _wikiParseService = new Lazy<IWikiParseService>(() => new WikiParseService(_wikiDataService.Value));
            _characterService = new Lazy<ICharacterService>(() => new CharacterService(characterRepository, _mapper));
            _characterCreationService = new Lazy<ICharacterCreationService>(() => new CharacterCreationService());
            _errorService = new Lazy<IErrorService>(() => new ErrorService(characterDatabaseFile));
            _increaseCalculationService = new Lazy<IIncreaseCalculationService>(() => new IncreaseCalculationService());
            _skillCalculationService = new Lazy<ISkillCalculationService>(() => new SkillCalculationService(_increaseCalculationService.Value));
            _skillGroupCalculationService = new Lazy<ISkillGroupCalculationService>(() => new SkillGroupCalculationService(_skillCalculationService.Value, _increaseCalculationService.Value));
            _attributeCalculationService = new Lazy<IAttributeCalculationService>(() => new AttributeCalculationService(_increaseCalculationService.Value, _skillGroupCalculationService.Value));
        }

        public IMapper Mapper()
        {
            return _mapper;
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

        public IErrorService ErrorService()
        {
            return _errorService.Value;
        }

        public IAttributeCalculationService AttributeCalculationService()
        {
            return _attributeCalculationService.Value;
        }

        public ISkillGroupCalculationService SkillGroupCalculationService()
        {
            return _skillGroupCalculationService.Value;
        }

        public ISkillCalculationService SkillCalculationService()
        {
            return _skillCalculationService.Value;
        }
    }
}