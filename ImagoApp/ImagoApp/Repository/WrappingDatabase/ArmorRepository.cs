using System.Threading.Tasks;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface IArmorRepository : IObjectJsonRepository<Models.ArmorPartModel, Models.Entity.ArmorPartEntity>
    {
        Task EnsureTables();
    }

    public class ArmorRepository : ObjectJsonRepositoryBase<Models.ArmorPartModel, Models.Entity.ArmorPartEntity>, IArmorRepository
    {
        public ArmorRepository(string databaseFolder) : base(databaseFolder, "Imago_Armor.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.ArmorPartEntity>();
        }
    }
}