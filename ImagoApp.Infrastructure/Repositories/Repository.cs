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
        int GetItemCount();
        FileInfo GetDatabaseInfo();
    }

    public abstract class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly string _databaseFile;
        protected readonly string ConnectionString;

        protected Repository(string databaseFile)
        {
            _databaseFile = databaseFile;

            //https://github.com/mbdavid/LiteDB/wiki/Connection-String
            ConnectionString = $"filename={_databaseFile}";

            var mapper = BsonMapper.Global;
            mapper.EnumAsInteger = true;
        }
        
        public FileInfo GetDatabaseInfo()
        {
            return new FileInfo(_databaseFile);
        }

        public int GetItemCount()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                return collection.Count();
            }
        }

        public virtual List<T> GetAllItems()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                return collection.FindAll().ToList();
            }
        }

        public void DeleteAll()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                db.DropCollection(typeof(T).Name);
            }
        }

        public void InsertBulk(IEnumerable<T> items)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                var count = collection.InsertBulk(items);
            }
        }
    }
}