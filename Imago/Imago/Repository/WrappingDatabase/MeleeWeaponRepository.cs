using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Imago.Database;
using Imago.Models;
using Imago.Models.Entity;
using Newtonsoft.Json;
using SQLite;

namespace Imago.Repository.WrappingDatabase
{
    public interface IMeleeWeaponRepository : IObjectJsonRepository<Weapon>
    {
        Task EnsureTables();
    }

    public class MeleeWeaponRepository : ObjectJsonRepositoryBase<Weapon, WeaponEntity>, IMeleeWeaponRepository
    {
        public MeleeWeaponRepository(string databaseFolder) : base(databaseFolder, "Imago_MeleeWeapons.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<WeaponEntity>();
        }
    }
}