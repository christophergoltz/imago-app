using System.Threading.Tasks;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface ITalentRepository : IObjectJsonRepository<Models.TalentModel, Models.Entity.TalentEntity>
    {
        Task EnsureTables();
    }

    public class TalentRepository : ObjectJsonRepositoryBase<Models.TalentModel, Models.Entity.TalentEntity>, ITalentRepository
    {
        public TalentRepository(string databaseFolder) : base(databaseFolder, "Imago_Talents.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.TalentEntity>();
        }
    }
}
