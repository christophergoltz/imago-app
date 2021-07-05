using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAllItems();
        void DeleteAll();
        void InsertBulk(IEnumerable<T> items);
    }

    public abstract class Repository<T> : IRepository<T> where T : class, new()
    {
        private string _connectionString; // db;

        public Repository(string databaseFolder)
        {
            //var databaseDirectory = Path.GetDirectoryName(databaseFolder);
            //if (databaseDirectory != null && !Directory.Exists(databaseDirectory))
            //    Directory.CreateDirectory(databaseDirectory);

            var databaseFile = Path.Combine(databaseFolder, "ImagoApp_Armor.db");

            //https://github.com/mbdavid/LiteDB/wiki/Connection-String
            _connectionString = $"filename={databaseFile}";

            //   db = new LiteDatabase(connString);

            //BsonMapper mapper = BsonMapper.Global;
            //mapper.EnumAsInteger = true;
            //mapper.Entity<T>().Id(p => p.Guid);
        }

        public virtual List<T> GetAllItems()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                return collection.FindAll().ToList();
            }
        }
        
        public void DeleteAll()
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                db.DropCollection(typeof(T).Name);
            }
        }

        public void InsertBulk(IEnumerable<T> items)
        {
            using (var db = new LiteDatabase(_connectionString))
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                foreach (var item in items)
                {
                    collection.Insert(item);
                }
            }
        }
    }
}