using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imago.Models;
using Imago.Models.Entity;
using Imago.Models.Enum;
using SQLite;

namespace Imago.Database
{
    public class DatabaseInfoRepository
    {
        private static SQLiteAsyncConnection _database;

        public static readonly AsyncLazy<DatabaseInfoRepository> Instance = new AsyncLazy<DatabaseInfoRepository>(async () =>
        {
            var instance = new DatabaseInfoRepository();
            await _database.CreateTableAsync<ArmorSetEntity>();
            await _database.CreateTableAsync();
            return instance;
        });

        public DatabaseInfoRepository()
        {
            Debug.WriteLine("Database: " + DatabaseConstants.DatabasePath);
            _database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        }
        
        public async Task<List<TableInfoModel>> GetAllTableInfos()
        {
            var result = await _database.Table<TableInfoEntity>().ToListAsync();

            var s = ((TableInfoType[])Enum.GetValues(typeof(TableInfoType))).ToList();
        
            foreach (var entity in result)
            {
                s.Remove(entity.Type);
            }

            var missingPart = false;

            foreach (var missingType in s)
            {
                await _database.InsertAsync(new TableInfoEntity() {TimeStamp = null, Type = missingType});
                missingPart = true;
            }

            if(missingPart)
                result = await _database.Table<TableInfoEntity>().ToListAsync();

            var l = new List<TableInfoModel>();

            foreach (var tableInfoEntity in result)
            {
                var model = App.Mapper.Map<TableInfoModel>(tableInfoEntity);
                int count = -1;

                switch (tableInfoEntity.Type)
                {
                    case TableInfoType.Armor:
                        count = await _database.Table<ArmorSetEntity>().CountAsync();
                        break;
                    case TableInfoType.Weapons:
                       
                        break;
                }

                model.Count = count;
              l.Add(model);
            }

            return l;
        }

        //public Task<int> SaveItemAsync(TableInfoEntity item)
        //{
        //    if (item.Id != 0)
        //    {
        //        return _database.UpdateAsync(item);
        //    }
        //    else
        //    {
        //        return _database.InsertAsync(item);
        //    }
        //}

        public void DeleteAllArmor()
        {
            _database.DeleteAllAsync<ArmorSetEntity>();
        }

        public void DeleteAllMeleeWeapons()
        {
            _database.DeleteAllAsync<Weaent>();
        }

        public async Task InsertMany(List<ArmorSet>sets )
        {
            var w = App.Mapper.Map<List<ArmorSetEntity>>(sets);
            await _database.InsertAllAsync(w);
        }

        public async Task Update(TableInfoModel talbTableInfoModel)
        {
            var w = App.Mapper.Map<TableInfoEntity>(talbTableInfoModel);
            await _database.UpdateAsync(w);
        }

        public Task<int> DeleteItemAsync(TableInfoEntity item)
        {
            
            return _database.DeleteAsync(item);
        }
    }
}
