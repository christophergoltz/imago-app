using System.Collections.Generic;
using System.IO;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Template;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Entities.Template;
using ImagoApp.Infrastructure.Repositories;

namespace ImagoApp.Application.Services
{
    public interface IWikiDataService
    {
        void AddWeapons(List<WeaponTemplateModel> items);
        List<WeaponTemplateModel> GetAllWeapons();
        void DeleteAllWeapons();
        void AddArmor(List<ArmorPartTemplateModel> items);
        List<ArmorPartTemplateModel> GetAllArmor();
        void DeleteAllArmor();
        void AddMasteries(List<MasteryModel> items);
        List<MasteryModel> GetAllMasteries();
        void DeleteAllMasteries();
        void AddTalents(List<TalentModel> items);
        List<TalentModel> GetAllTalents();
        void DeleteAllTalents();
        (int ItemCount, FileInfo FileInfo) GetDatabaseInfo();
        Weapon GetWeaponFromTemplate(WeaponTemplateModel model);
        ArmorPartModel GetArmorFromTemplate(ArmorPartTemplateModel model);
    }

    public class WikiDataService : IWikiDataService
    {
        private readonly IArmorTemplateRepository _armorTemplateRepository;
        private readonly IWeaponTemplateRepository _weaponTemplateRepository;
        private readonly ITalentRepository _talentRepository;
        private readonly IMasteryRepository _masteryRepository;
        private readonly IMapper _mapper;

        public WikiDataService(IMapper mapper, 
            IArmorTemplateRepository armorTemplateRepository,
            IWeaponTemplateRepository weaponTemplateRepository,
            ITalentRepository talentRepository,
            IMasteryRepository masteryRepository)
        {
            _armorTemplateRepository = armorTemplateRepository;
            _weaponTemplateRepository = weaponTemplateRepository;
            _talentRepository = talentRepository;
            _masteryRepository = masteryRepository;
            _mapper = mapper;
        }

        public Weapon GetWeaponFromTemplate(WeaponTemplateModel model)
        {
            return new Weapon(model.Name, model.WeaponStances, true, true, model.LoadValue, model.DurabilityValue);
        }

        public ArmorPartModel GetArmorFromTemplate(ArmorPartTemplateModel model)
        {
            return new ArmorPartModel(model.ArmorPartType, model.Name, model.LoadValue,true, true, model.DurabilityValue, model.EnergyDefense, model.PhysicalDefense);
        }

        public (int ItemCount, FileInfo FileInfo) GetDatabaseInfo()
        {
            return (0, null); // _wikiDataRepository.GetDatabaseInfo();
        }

        #region Weapons

        public void AddWeapons(List<WeaponTemplateModel> items)
        {
            var entities = _mapper.Map<List<WeaponTemplateEntity>>(items);
            _weaponTemplateRepository.InsertBulk(entities);
        }

        public List<WeaponTemplateModel> GetAllWeapons()
        {
            var entities = _weaponTemplateRepository.GetAllItems();
            var models = _mapper.Map<List<WeaponTemplateModel>>(entities);
            return models;
        }

        public void DeleteAllWeapons()
        {
            _weaponTemplateRepository.DeleteAll();
        }

        #endregion

        #region Armor

        public void AddArmor(List<ArmorPartTemplateModel> items)
        {
            var entities = _mapper.Map<List<ArmorPartTemplateEntity>>(items);
            _armorTemplateRepository.InsertBulk(entities);
        }

        public List<ArmorPartTemplateModel> GetAllArmor()
        {
            var entities = _armorTemplateRepository.GetAllItems();
            var models = _mapper.Map<List<ArmorPartTemplateModel>>(entities);
            return models;
        }

        public void DeleteAllArmor()
        {
            _armorTemplateRepository.DeleteAll();
        }

        #endregion

        #region Masteries

        public void AddMasteries(List<MasteryModel> items)
        {
            var entities = _mapper.Map<List<MasteryEntity>>(items);
            _masteryRepository.InsertBulk(entities);
        }

        public List<MasteryModel> GetAllMasteries()
        {
            var entites = _masteryRepository.GetAllItems();
            var weapons = _mapper.Map<List<MasteryModel>>(entites);
            return weapons;
        }

        public void DeleteAllMasteries()
        {
            _masteryRepository.DeleteAll();
        }

        #endregion

        #region Talents

        public void AddTalents(List<TalentModel> items)
        {
            var entities = _mapper.Map<List<TalentEntity>>(items);
            _talentRepository.InsertBulk(entities);
        }

        public List<TalentModel> GetAllTalents()
        {
            var entities = _talentRepository.GetAllItems();
            var weapons = _mapper.Map<List<TalentModel>>(entities);
            return weapons;
        }

        public void DeleteAllTalents()
        {
            _talentRepository.DeleteAll();
        }

        #endregion
    }
}