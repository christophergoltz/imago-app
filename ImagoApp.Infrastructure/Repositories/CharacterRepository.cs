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
        bool InsertItem(CharacterEntity item, string databaseFile);
        bool UpdateItem(CharacterEntity item);
        CharacterEntity GetItem(Guid id, string databaseFile);
        string GetCharacterJson(Guid guid);
        CharacterPreview GetItemAsPreview();
        void UpdateLastBackup(string databaseFile);
    }

    public class CharacterRepository : ICharacterRepository
    {
        private readonly Func<string> _getDatabaseFile;
        private readonly ICharacterDatabaseConnection _characterDatabaseConnection;
        private string ConnectionString => $"filename={DatabaseFile}";
        private string DatabaseFile => _getDatabaseFile.Invoke();
        private const string CollectionName = nameof(CharacterEntity);

        public CharacterRepository(Func<string> getDatabaseFile, ICharacterDatabaseConnection characterDatabaseConnection)
        {
            //https://github.com/mbdavid/LiteDB/wiki/Connection-String
            _getDatabaseFile = getDatabaseFile;
            _characterDatabaseConnection = characterDatabaseConnection;

            var mapper = BsonMapper.Global;
            mapper.EnumAsInteger = true;
            mapper.Entity<CharacterEntity>().Id(p => p.Guid);
        }

        public bool InsertItem(CharacterEntity item, string databaseFile)
        {
            using (var db = new LiteDatabase($"filename={databaseFile}"))
            {
                item.LastEdit = DateTime.Now;
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                collection.Insert(item);

                return true;
            }
        }

        public string GetCharacterJson(Guid guid)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                var item = collection.FindById(guid);
                return JsonConvert.SerializeObject(item);
            }
        }

        public bool UpdateItem(CharacterEntity item)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                item.LastEdit = DateTime.Now;
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                return collection.Update(item);
            }
        }

        public void UpdateLastBackup(string databaseFile)
        {
            using (var db = new LiteDatabase($"filename={databaseFile}"))
            {
                //https://www.litedb.org/api/update/
                var command = $"UPDATE {CollectionName} SET LastBackup = NOW()";
                db.Execute(command);
            }
        }

        public CharacterEntity GetItem(Guid id, string databaseFile)
        {
            using (var db = new LiteDatabase($"filename={databaseFile}"))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                return collection.FindById(id);
            }
        }

        public CharacterPreview GetItemAsPreview()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var collection = db.GetCollection<CharacterEntity>(CollectionName);
                var res = collection.Query().First();

                var filePath = _characterDatabaseConnection.GetCharacterDatabaseFile(res.Guid);
                var fileInfo = new FileInfo(DatabaseFile);

                return new CharacterPreview(res.Guid, res.Name, res.Version, res.LastEdit, filePath, fileInfo.Length, res.LastBackup);
            }
        }
    }
}