using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.Infrastructure.Database;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Shared;
using LiteDB;
using Newtonsoft.Json;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface ICharacterRepository
    {
        bool InsertItem(CharacterEntity item);
        bool UpdateItem(CharacterEntity item);
        CharacterEntity GetItem(Guid guid);
        string GetCharacterJson(Guid guid);
        CharacterPreview GetItemAsPreview(string databaseFile);
        CharacterPreview GetItemAsPreview(Guid guid);
        void UpdateLastBackup(Guid guid);
    }

    public class CharacterRepository : ICharacterRepository
    {
        private readonly ICharacterDatabaseConnection _characterDatabaseConnection;
        private const string CollectionName = nameof(CharacterEntity);

        public CharacterRepository(ICharacterDatabaseConnection characterDatabaseConnection)
        {
            _characterDatabaseConnection = characterDatabaseConnection;

            var mapper = BsonMapper.Global;
            mapper.EnumAsInteger = true;
            mapper.Entity<CharacterEntity>().Id(p => p.Guid);
        }

        public bool InsertItem(CharacterEntity item)
        {
            using (var db = new LiteDatabase(_characterDatabaseConnection.GetDatabaseConnectionString(item.Guid)))
            {
                item.LastEdit = DateTime.Now;
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                collection.Insert(item);

                return true;
            }
        }

        public string GetCharacterJson(Guid guid)
        {
            using (var db = new LiteDatabase(_characterDatabaseConnection.GetDatabaseConnectionString(guid)))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                var item = collection.FindById(guid);
                return JsonConvert.SerializeObject(item);
            }
        }

        public bool UpdateItem(CharacterEntity item)
        {
            using (var db = new LiteDatabase(_characterDatabaseConnection.GetDatabaseConnectionString(item.Guid)))
            {
                item.LastEdit = DateTime.Now;
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                return collection.Update(item);
            }
        }

        public void UpdateLastBackup(Guid guid)
        {
            using (var db = new LiteDatabase(_characterDatabaseConnection.GetDatabaseConnectionString(guid)))
            {
                //https://www.litedb.org/api/update/
                var command = $"UPDATE {CollectionName} SET LastBackup = NOW()";
                db.Execute(command);
            }
        }

        public CharacterEntity GetItem(Guid guid)
        {
            using (var db = new LiteDatabase(_characterDatabaseConnection.GetDatabaseConnectionString(guid)))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                return collection.FindById(guid);
            }
        }

        public CharacterPreview GetItemAsPreview(Guid guid)
        {
            var databaseFile = _characterDatabaseConnection.GetCharacterDatabaseFile(guid);
            if (!File.Exists(databaseFile))
                return null;

            using (var db = new LiteDatabase($"filename={databaseFile}"))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                var res = collection.Query().First();
                var fileInfo = new FileInfo(databaseFile);

                return new CharacterPreview(res.Guid, res.Name, res.Version, res.LastEdit, databaseFile,
                    fileInfo.Length, res.LastBackup);
            }
        }

        public CharacterPreview GetItemAsPreview(string databaseFile)
        {
            using (var db = new LiteDatabase($"filename={databaseFile}"))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                var res = collection.Query().First();
                var fileInfo = new FileInfo(databaseFile);

                return new CharacterPreview(res.Guid, res.Name, res.Version, res.LastEdit, databaseFile, fileInfo.Length, res.LastBackup);
            }
        }
    }
}