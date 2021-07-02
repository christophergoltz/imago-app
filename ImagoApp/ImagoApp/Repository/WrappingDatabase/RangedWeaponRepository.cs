using System.Threading.Tasks;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface IRangedWeaponRepository : IObjectJsonRepository<Models.Weapon, Models.Entity.WeaponEntity>
    {
        Task EnsureTables();
    }

    public class RangedWeaponRepository : ObjectJsonRepositoryBase<Models.Weapon, Models.Entity.WeaponEntity>, IRangedWeaponRepository
    {
        public RangedWeaponRepository(string databaseFolder) : base(databaseFolder, "Imago_RangedWeapons.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.WeaponEntity>();
        }
    }
}
