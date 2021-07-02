using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImagoApp.Database;
using SQLite;

namespace ImagoApp.Repository
{
    public class WikiRepository
    {
        private readonly string _databaseFolder;
        private readonly string _databaseFile;
        protected readonly SQLiteAsyncConnection Database;

        protected WikiRepository(string databaseFolder, string databaseFile)
        {
            _databaseFolder = databaseFolder;
            _databaseFile = Path.Combine(_databaseFolder, databaseFile);
            Database = new SQLiteAsyncConnection(_databaseFile, DatabaseConstants.Flags);
        }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.ArmorPartEntity>();
        }

        public DateTime GetLastChangedDate()
        {
            return new FileInfo(_databaseFile).LastWriteTime;
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
