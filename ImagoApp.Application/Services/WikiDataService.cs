using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Repositories;

namespace ImagoApp.Application.Services
{
    public interface IWikiDataService
    {
        Task<int> AddWeapons(List<Weapon> items);
        Task<List<Weapon>> GetAllWeapons();
        Task DeleteAllWeapons();
        Task<int> AddArmor(List<ArmorPartModel> items);
        Task<List<ArmorPartModel>> GetAllArmor();
        Task DeleteAllArmor();
        Task<int> AddMasteries(List<MasteryModel> items);
        Task<List<MasteryModel>> GetAllMasteries();
        Task DeleteAllMasteries();
        Task<int> AddTalents(List<TalentModel> items);
        Task<List<TalentModel>> GetAllTalents();
        Task DeleteAllTalents();
        Task Initialize();
        Task<(int ItemCount, FileInfo FileInfo)> GetDatabaseInfo();
    }

    public class WikiDataService : IWikiDataService
    {
        private readonly IWikiDataRepository _wikiDataRepository;
        private readonly IMapper _mapper;

        public WikiDataService(IWikiDataRepository wikiDataRepository, IMapper mapper)
        {
            _wikiDataRepository = wikiDataRepository;
            _mapper = mapper;
        }

        public async Task Initialize()
        {
            await _wikiDataRepository.EnsureTables();
        }

        public async Task<(int ItemCount, FileInfo FileInfo)> GetDatabaseInfo()
        {
            return await _wikiDataRepository.GetDatabaseInfo();
        }

        #region Weapons

        public async Task<int> AddWeapons(List<Weapon> items)
        {
            var entities = _mapper.Map<List<WeaponEntity>>(items);
            return await _wikiDataRepository.AddAllItems(entities);
        }

        public async Task<List<Weapon>> GetAllWeapons()
        {
            var entites = await _wikiDataRepository.GetAllItemsAsync<WeaponEntity>();
            var models = _mapper.Map<List<Weapon>>(entites);
            return models;
        }

        public async Task DeleteAllWeapons()
        {
            await _wikiDataRepository.DeleteAllItems<WeaponEntity>();
        }

        #endregion

        #region Armor

        public async Task<int> AddArmor(List<ArmorPartModel> items)
        {
            var entities = _mapper.Map<List<ArmorPartEntity>>(items);
            return await _wikiDataRepository.AddAllItems(entities);
        }

        public async Task<List<ArmorPartModel>> GetAllArmor()
        {
            var entites = await _wikiDataRepository.GetAllItemsAsync<ArmorPartEntity>();
            var models = _mapper.Map<List<ArmorPartModel>>(entites);
            return models;
        }

        public async Task DeleteAllArmor()
        {
            await _wikiDataRepository.DeleteAllItems<ArmorPartEntity>();
        }

        #endregion

        #region Masteries

        public async Task<int> AddMasteries(List<MasteryModel> items)
        {
            var entities = _mapper.Map<List<MasteryEntity>>(items);
            return await _wikiDataRepository.AddAllItems(entities);
        }

        public async Task<List<MasteryModel>> GetAllMasteries()
        {
            var entites = await _wikiDataRepository.GetAllItemsAsync<MasteryEntity>();
            var weapons = _mapper.Map<List<MasteryModel>>(entites);
            return weapons;
        }

        public async Task DeleteAllMasteries()
        {
            await _wikiDataRepository.DeleteAllItems<MasteryEntity>();
        }

        #endregion

        #region Talents

        public async Task<int> AddTalents(List<TalentModel> items)
        {
            var entities = _mapper.Map<List<TalentEntity>>(items);
            return await _wikiDataRepository.AddAllItems(entities);
        }

        public async Task<List<TalentModel>> GetAllTalents()
        {
            var entites = await _wikiDataRepository.GetAllItemsAsync<TalentEntity>();
            var weapons = _mapper.Map<List<TalentModel>>(entites);
            return weapons;
        }

        public async Task DeleteAllTalents()
        {
            await _wikiDataRepository.DeleteAllItems<TalentEntity>();
        }

        #endregion
    }
}