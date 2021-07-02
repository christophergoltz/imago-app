using System.Threading.Tasks;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface IMasteryRepository : IObjectJsonRepository<Models.MasteryModel, Models.Entity.MasteryEntity>
    {
        Task EnsureTables();
    }

    public class MasteryRepository : ObjectJsonRepositoryBase<Models.MasteryModel, Models.Entity.MasteryEntity>, IMasteryRepository
    {
        public MasteryRepository(string databaseFolder) : base(databaseFolder, "Imago_Mastery.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.MasteryEntity>();
        }
    }
}
