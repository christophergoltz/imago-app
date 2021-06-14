using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imago.Models;
using Imago.Models.Entity;

namespace Imago.Repository.WrappingDatabase
{
    public interface IShieldRepository : IObjectJsonRepository<Weapon>
    {
        Task EnsureTables();
    }

    public class ShieldRepository : ObjectJsonRepositoryBase<Weapon, WeaponEntity>, IShieldRepository
    {
        public ShieldRepository(string databaseFolder) : base(databaseFolder, "Imago_Shields.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<WeaponEntity>();
        }
    }
}
