using System.Threading.Tasks;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface IShieldRepository : IObjectJsonRepository<Models.Weapon, Models.Entity.WeaponEntity>
    {
        Task EnsureTables();
    }

    public class ShieldRepository : ObjectJsonRepositoryBase<Models.Weapon, Models.Entity.WeaponEntity>, IShieldRepository
    {
        public ShieldRepository(string databaseFolder) : base(databaseFolder, "Imago_Shields.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.WeaponEntity>();
        }
    }
}
