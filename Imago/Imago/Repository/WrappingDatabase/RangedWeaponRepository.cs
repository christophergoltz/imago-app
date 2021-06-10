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
    public class RangedWeaponRepository : ObjectJsonRepositoryBase<Weapon, WeaponEntity>, IRangedWeaponRepository
    {
        public RangedWeaponRepository(string databaseFolder) : base(databaseFolder, "Imago_RangedWeapons.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<WeaponEntity>();
        }
    }
}
