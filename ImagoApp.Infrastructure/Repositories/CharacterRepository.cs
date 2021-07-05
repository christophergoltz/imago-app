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
    }

    public class CharacterRepository : ICharacterRepository
    {
        private string _connectionString;
        private string _collectionName = nameof(CharacterEntity);

        public CharacterRepository(string databaseFolder)
        {
            //var databaseDirectory = Path.GetDirectoryName(databaseFolder);
            //if (databaseDirectory != null && !Directory.Exists(databaseDirectory))
            //    Directory.CreateDirectory(databaseDirectory);

            var databaseFile = Path.Combine(databaseFolder, "ImagoApp_Character.db");

            //https://github.com/mbdavid/LiteDB/wiki/Connection-String
            _connectionString = $"filename={databaseFile}";

            //   db = new LiteDatabase(connString);

            BsonMapper mapper = BsonMapper.Global;
            mapper.EnumAsInteger = true;
            mapper.Entity<CharacterEntity>().Id(p => p.Guid);
        }

        public bool InsertItem(CharacterEntity item)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                item.LastEdit = DateTime.Now;
                var collection = db.GetCollection<CharacterEntity>(_collectionName);
                collection.Insert(item);

                return true;
            }
        }

        public bool UpdateItem(CharacterEntity item)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                item.LastEdit = DateTime.Now;
                var collection = db.GetCollection<CharacterEntity>(_collectionName);
                var result = collection.Update(item);

                return result;
            }
        }

        public bool DeleteItem(Guid guid)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<CharacterEntity>(_collectionName);
                var result = collection.Delete(guid);
                return result;
            }
        }

        public CharacterEntity GetItem(Guid id)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<CharacterEntity>(_collectionName);
                return collection.FindById(id);
            }
        }

        public List<CharacterEntity> GetAllItems()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<CharacterEntity>(_collectionName);
                return collection.FindAll().ToList();
            }
        }
    }
}