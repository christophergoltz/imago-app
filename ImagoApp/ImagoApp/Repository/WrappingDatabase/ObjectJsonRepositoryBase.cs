using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImagoApp.Database;
using SQLite;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface IObjectJsonRepository<TModel, TEntity>
        where TModel : class, new()
    {
        DateTime GetLastChangedDate();
        Task<int> GetItemsCount();
        Task<List<TModel>> GetAllItemsAsync();
        Task DeleteAllItems();
        Task<int> AddAllItems(IEnumerable<TModel> items);
        Task<List<TEntity>> GetAllItemsRawAsync();
        Task<int> AddItemRawAsync(TEntity item);
    }

    public abstract class ObjectJsonRepositoryBase<TModel, TEntity> : IObjectJsonRepository<TModel, TEntity>
        where TModel : class, new()
        where TEntity : Models.Entity.IJsonValueWrapper<TModel>, new()
    {
        private readonly string _databaseFolder;
        private readonly string _databaseFile;
        protected readonly SQLiteAsyncConnection Database;

        protected ObjectJsonRepositoryBase(string databaseFolder, string databaseFile)
        {
            _databaseFolder = databaseFolder;
            _databaseFile = Path.Combine(_databaseFolder, databaseFile);
            Database = new SQLiteAsyncConnection(_databaseFile, DatabaseConstants.Flags);
        }

        public DateTime GetLastChangedDate()
        {
            return new FileInfo(_databaseFile).LastWriteTime;
        }

        public async Task<int> GetItemsCount()
        {
            return await Database.Table<TEntity>().CountAsync();
        }

        public async Task<List<TModel>> GetAllItemsAsync()
        {
            var items = await Database.Table<TEntity>().ToListAsync();
            return items.Select(entity => entity.Value).ToList();
        }

        public async Task<List<TEntity>> GetAllItemsRawAsync()
        {
            var items = await Database.Table<TEntity>().ToListAsync();
            return items;
        }

        public Task DeleteAllItems()
        {
            return Database.DeleteAllAsync<TEntity>();
        }

        public Task<int> AddAllItems(IEnumerable<TModel> items)
        {
            var entites = items.Select(model => new TEntity {Value = model}).ToList();
            return Database.InsertAllAsync(entites);
        }

        public Task<int> AddItemRawAsync(TEntity item)
        {
            return Database.InsertAsync(item);
        }
    }
}