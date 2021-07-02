using System.Threading.Tasks;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface IMeleeWeaponRepository : IObjectJsonRepository<Models.Weapon, Models.Entity.WeaponEntity>
    {
        Task EnsureTables();
    }

    public class MeleeWeaponRepository : ObjectJsonRepositoryBase<Models.Weapon, Models.Entity.WeaponEntity>, IMeleeWeaponRepository
    {
        public MeleeWeaponRepository(string databaseFolder) : base(databaseFolder, "Imago_MeleeWeapons.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.WeaponEntity>();
        }
    }
}