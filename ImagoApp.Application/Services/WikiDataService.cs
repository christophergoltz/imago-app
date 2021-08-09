using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Template;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Entities.Template;
using ImagoApp.Infrastructure.Repositories;
using ImagoApp.Shared.Enums;

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
        WeaponModel GetWeaponFromTemplate(WeaponTemplateModel model);
        ArmorPartModelModel GetArmorFromTemplate(ArmorPartTemplateModel model);
        int GetArmorWikiDataItemCount();
        int GetWeaponWikiDataItemCount();
        int GetTalentWikiDataItemCount();
        int GetMasteryWikiDataItemCount();
        FileInfo GetDatabaseInfo();
        void AddWeaveTalents(List<WeaveTalentModel> items);
        List<WeaveTalentModel> GetAllWeaveTalents();
        void DeleteAllWeaveTalents();
        int GetWeaveTalentWikiDataItemCount();
        List<MasteryModel> GetAllMasteries(SkillGroupModelType groupModelType);
    }

    public class WikiDataService : IWikiDataService
    {
        private readonly IArmorTemplateRepository _armorTemplateRepository;
        private readonly IWeaponTemplateRepository _weaponTemplateRepository;
        private readonly ITalentRepository _talentRepository;
        private readonly IMasteryRepository _masteryRepository;
        private readonly IWeaveTalentRepository _weaveTalentRepository;
        private readonly IMapper _mapper;

        public WikiDataService(IMapper mapper, 
            IArmorTemplateRepository armorTemplateRepository,
            IWeaponTemplateRepository weaponTemplateRepository,
            ITalentRepository talentRepository,
            IMasteryRepository masteryRepository,
            IWeaveTalentRepository weaveTalentRepository)
        {
            _armorTemplateRepository = armorTemplateRepository;
            _weaponTemplateRepository = weaponTemplateRepository;
            _talentRepository = talentRepository;
            _masteryRepository = masteryRepository;
            _weaveTalentRepository = weaveTalentRepository;
            _mapper = mapper;
        }

        public int GetArmorWikiDataItemCount()
        {
            return _armorTemplateRepository.GetItemCount(); 
        }

        public int GetWeaponWikiDataItemCount()
        {
            return _weaponTemplateRepository.GetItemCount();
        }

        public int GetTalentWikiDataItemCount()
        {
            return _talentRepository.GetItemCount();
        }

        public int GetWeaveTalentWikiDataItemCount()
        {
            return _weaveTalentRepository.GetItemCount();
        }

        public int GetMasteryWikiDataItemCount()
        {
            return _masteryRepository.GetItemCount();
        }

        public WeaponModel GetWeaponFromTemplate(WeaponTemplateModel model)
        {
            return new WeaponModel(model.Name, model.WeaponStances, true, true, model.LoadValue, model.DurabilityValue);
        }

        public ArmorPartModelModel GetArmorFromTemplate(ArmorPartTemplateModel model)
        {
            return new ArmorPartModelModel(model.ArmorPartType, model.Name, model.LoadValue,true, true, model.DurabilityValue, model.EnergyDefense, model.PhysicalDefense);
        }

        public FileInfo GetDatabaseInfo()
        {
            return _armorTemplateRepository.GetDatabaseInfo();
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

        public List<MasteryModel> GetAllMasteries(SkillGroupModelType groupModelType)
        {
            var entites = _masteryRepository.GetAllMasteries(groupModelType);
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
            var talents = _mapper.Map<List<TalentModel>>(entities);
            return talents;
        }

        public void DeleteAllTalents()
        {
            _talentRepository.DeleteAll();
        }

        #endregion

        #region WeaveTalents

        public void AddWeaveTalents(List<WeaveTalentModel> items)
        {
            var entities = _mapper.Map<List<WeaveTalentEntity>>(items);
            _weaveTalentRepository.InsertBulk(entities);
        }

        public List<WeaveTalentModel> GetAllWeaveTalents()
        {
            var entities = _weaveTalentRepository.GetAllItems();
            var weaveTalents = _mapper.Map<List<WeaveTalentModel>>(entities);
            return weaveTalents;
        }

        public void DeleteAllWeaveTalents()
        {
            _weaveTalentRepository.DeleteAll();
        }

        #endregion
    }
}