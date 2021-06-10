using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imago.Database;
using Imago.Models;
using Imago.Models.Entity;
using Newtonsoft.Json;
using SQLite;

namespace Imago.Repository.WrappingDatabase
{
    public class ArmorRepository : IWrappingRepository<ArmorSet, ArmorSetEntity>
    {
        private static SQLiteAsyncConnection _database;
        private static string _databaseFile;

        public static async Task<ArmorRepository> Setup(string databasePath)
        {
            _databaseFile = Path.Combine(databasePath, "Imago_Armor.db3");
            _database = new SQLiteAsyncConnection(_databaseFile, DatabaseConstants.Flags);
            await _database.CreateTableAsync<ArmorSetEntity>();
            return new ArmorRepository();
        }

        private ArmorRepository()
        {
        }

        public DateTime GetLastChangedDate()
        {
            return new FileInfo(_databaseFile).LastWriteTime;
        }

        public async Task<int> GetItemsCount()
        {
            return await _database.Table<ArmorSetEntity>().CountAsync();
        }

        public async Task<List<ArmorSet>> GetAllItemsAsync()
        {
            var items = await _database.Table<ArmorSetEntity>().ToListAsync();
            return items.Select(entity => entity.MapToModel()).ToList();
        }

        public Task DeleteAllItems()
        {
            return _database.DeleteAllAsync<ArmorSetEntity>();
        }

        public Task AddAllItems(IEnumerable<ArmorSet> items)
        {
            var entities = items.Select(JsonConvert.SerializeObject);
            return _database.InsertAllAsync(entities);
        }
    }
}