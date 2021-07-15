using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.Infrastructure.Entities;
using LiteDB;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface ICharacterRepository
    {
        bool InsertItem(CharacterEntity item);
        bool UpdateItem(CharacterEntity item);
        bool DeleteItem(Guid guid);
        CharacterEntity GetItem(Guid id);
        List<CharacterEntity> GetAllItems();
        FileInfo GetDatabaseInfo();
    }

    public class CharacterRepository : ICharacterRepository
    {
        private readonly string _connectionString;
        private const string CollectionName = nameof(CharacterEntity);

        public CharacterRepository(string databaseFile)
        {
            //https://github.com/mbdavid/LiteDB/wiki/Connection-String
            _connectionString = $"filename={databaseFile}";
            
            var mapper = BsonMapper.Global;
            mapper.EnumAsInteger = true;
            mapper.Entity<CharacterEntity>().Id(p => p.Guid);
        }

        public bool InsertItem(CharacterEntity item)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                item.LastEdit = DateTime.Now;
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                collection.Insert(item);

                return true;
            }
        }

        public bool UpdateItem(CharacterEntity item)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                item.LastEdit = DateTime.Now;
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                return collection.Update(item);
            }
        }

        public bool DeleteItem(Guid guid)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                var result = collection.Delete(guid);
                return result;
            }
        }

        public CharacterEntity GetItem(Guid id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                return collection.FindById(id);
            }
        }

        public List<CharacterEntity> GetAllItems()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                return collection.FindAll().ToList();
            }
        }

        public FileInfo GetDatabaseInfo()
        {
            return new FileInfo(_databaseFile);
        }
    }
}