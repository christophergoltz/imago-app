using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imago.Models;
using Imago.Models.Entity;

namespace Imago.Repository.WrappingDatabase
{
    public interface ISpecialWeaponRepository : IObjectJsonRepository<Weapon, WeaponEntity>
    {
        Task EnsureTables();
    }

    public class SpecialWeaponRepository : ObjectJsonRepositoryBase<Weapon, WeaponEntity>, ISpecialWeaponRepository
    {
        public SpecialWeaponRepository(string databaseFolder) : base(databaseFolder, "Imago_SpecialWeapons.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<WeaponEntity>();
        }
    }
}
