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
    public class RangedWeaponRepository : IWrappingRepository<Weapon, WeaponEntity>
    {
        private static SQLiteAsyncConnection _database;

        public static async Task<RangedWeaponRepository> Setup(string databasePath)
        {
            var databaseFile = Path.Combine(databasePath, "Imago_RangedWeapons.db3");
            _database = new SQLiteAsyncConnection(databasePath, DatabaseConstants.Flags);
            await _database.CreateTableAsync<WeaponEntity>();
            return new RangedWeaponRepository();
        }

        private RangedWeaponRepository() { }

        public async Task<List<Weapon>> GetAllItemsAsync()
        {
            var items = await _database.Table<WeaponEntity>().ToListAsync();
            return items.Select(entity => entity.MapToModel()).ToList();
        }

        public Task DeleteAllItems()
        {
            return _database.DeleteAllAsync<WeaponEntity>();
        }

        public Task AddAllItems(IEnumerable<Weapon> items)
        {
            var entities = items.Select(JsonConvert.SerializeObject);
            return _database.InsertAllAsync(entities);
        }
    }
}
