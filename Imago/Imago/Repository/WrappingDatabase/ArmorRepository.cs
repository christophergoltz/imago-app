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
    public interface IArmorRepository : IObjectJsonRepository<ArmorSet, ArmorSetEntity>
    {
        Task EnsureTables();
    }

    public class ArmorRepository : ObjectJsonRepositoryBase<ArmorSet, ArmorSetEntity>, IArmorRepository
    {
        public ArmorRepository(string databaseFolder) : base(databaseFolder, "Imago_Armor.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<ArmorSetEntity>();
        }
    }
}