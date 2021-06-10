using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imago.Database;
using Imago.Models;
using Imago.Models.Entity;
using Imago.Repository.WrappingDatabase;
using Newtonsoft.Json;
using SQLite;

namespace Imago.Repository
{
    public abstract class ObjectJsonRepositoryBase<TModel, TEntity> : IObjectJsonRepository<TModel>
        where TModel : class, new()
        where TEntity : IJsonValueWrapper<TModel>, new()
    {
        private readonly string _databaseFolder;
        private readonly string _databaseFile;
        protected readonly SQLiteAsyncConnection Database;

        protected ObjectJsonRepositoryBase(string databaseFolder, string databaseFile)
        {
            _databaseFolder = databaseFolder;
            _databaseFile = Path.Combine(_databaseFolder, databaseFile);
            Debug.WriteLine("Database: " + _databaseFile);
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

        public Task DeleteAllItems()
        {
            return Database.DeleteAllAsync<TEntity>();
        }

        public Task<int> AddAllItems(IEnumerable<TModel> items)
        {
            var entites = items.Select(model => new TEntity {Value = model}).ToList();
            return Database.InsertAllAsync(entites);
        }
    }
}