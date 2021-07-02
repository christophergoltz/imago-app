using System.Threading.Tasks;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface ISpecialWeaponRepository : IObjectJsonRepository<Models.Weapon, Models.Entity.WeaponEntity>
    {
        Task EnsureTables();
    }

    public class SpecialWeaponRepository : ObjectJsonRepositoryBase<Models.Weapon, Models.Entity.WeaponEntity>, ISpecialWeaponRepository
    {
        public SpecialWeaponRepository(string databaseFolder) : base(databaseFolder, "Imago_SpecialWeapons.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.WeaponEntity>();
        }
    }
}
