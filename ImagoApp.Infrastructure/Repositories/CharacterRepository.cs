using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.Infrastructure.Entities;
using SQLite;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface ICharacterRepository
    {
        Task EnsureTables();
        Task<List<CharacterEntity>> GetAllItems();
        Task<int> AddItem(CharacterEntity characterEntity);
        Task<int> UpdateItem(CharacterEntity characterEntity);
    }

    public class CharacterRepository : ICharacterRepository
    {
        protected readonly SQLiteAsyncConnection Database;

        public CharacterRepository(string databaseFolder)
        {
            var databaseFile = Path.Combine(databaseFolder, "ImagoApp_Character.db3");
            Database = new SQLiteAsyncConnection(databaseFile, DatabaseConstants.Flags);
        }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<CharacterEntity>();
        }

        public async Task<List<CharacterEntity>> GetAllItems()
        {
            return await Database.Table<CharacterEntity>().ToListAsync();
        }
        
        public Task<int> AddItem(CharacterEntity characterEntity)
        {
            return Database.InsertAsync(characterEntity);
        }

        public Task<int> UpdateItem(CharacterEntity characterEntity)
        {
            return Database.UpdateAsync(characterEntity);
        }

    }
}
