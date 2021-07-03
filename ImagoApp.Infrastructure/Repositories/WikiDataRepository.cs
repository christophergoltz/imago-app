using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImagoApp.Infrastructure.Entities;
using SQLite;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IWikiDataRepository
    {
        Task EnsureTables();
        Task<(int ItemCount, FileInfo FileInfo)> GetDatabaseInfo();
        Task<int> GetItemsCount<TEntity>() where TEntity : class, new();
        Task<List<TEntity>> GetAllItemsAsync<TEntity>() where TEntity : class, new();
        Task<List<TEntity>> GetAllItemsRawAsync<TEntity>() where TEntity : class, new();
        Task DeleteAllItems<TEntity>();
        Task<int> AddAllItems<TEntity>(IEnumerable<TEntity> items) where TEntity : class, new();
        Task<int> AddItemRawAsync<TEntity>(TEntity item) where TEntity : class, new();
    }

    public class WikiDataDataRepository : IWikiDataRepository
    {
        private readonly string _databaseFile;
        protected readonly SQLiteAsyncConnection Database;

        public WikiDataDataRepository(string databaseFolder)
        {
            _databaseFile = Path.Combine(databaseFolder, "ImagoApp_WikiData.db3");
            Database = new SQLiteAsyncConnection(_databaseFile, DatabaseConstants.Flags);
        }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<ArmorPartEntity>();
            await Database.CreateTableAsync<WeaponEntity>();
            await Database.CreateTableAsync<TalentEntity>();
            await Database.CreateTableAsync<MasteryEntity>();
        }

        public async Task<(int ItemCount, FileInfo FileInfo)> GetDatabaseInfo()
        {
            var count = await GetItemsCount<WeaponEntity>();
            count += await GetItemsCount<ArmorPartEntity>();
            count += await GetItemsCount<TalentEntity>();
            count += await GetItemsCount<MasteryEntity>();

            return (count, new FileInfo(_databaseFile));
        }

        public async Task<int> GetItemsCount<TEntity>() where TEntity : class, new()
        {
            return await Database.Table<TEntity>().CountAsync();
        }

        public async Task<List<TEntity>> GetAllItemsAsync<TEntity>() where TEntity : class, new()
        {
            var items = await Database.Table<TEntity>().ToListAsync();
            return items;
        }

        public async Task<List<TEntity>> GetAllItemsRawAsync<TEntity>() where TEntity : class, new()
        {
            var items = await Database.Table<TEntity>().ToListAsync();
            return items;
        }

        public Task DeleteAllItems<TEntity>()
        {
            return Database.DeleteAllAsync<TEntity>();
        }

        public Task<int> AddAllItems<TEntity>(IEnumerable<TEntity> items) where TEntity : class, new()
        {
            return Database.InsertAllAsync(items);
        }

        public Task<int> AddItemRawAsync<TEntity>(TEntity item) where TEntity : class, new()
        {
            return Database.InsertAsync(item);
        }
    }
}
