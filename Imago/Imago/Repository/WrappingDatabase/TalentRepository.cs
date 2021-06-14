using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imago.Models;
using Imago.Models.Entity;

namespace Imago.Repository.WrappingDatabase
{
    public interface ITalentRepository : IObjectJsonRepository<TalentModel>
    {
        Task EnsureTables();
    }

    public class TalentRepository : ObjectJsonRepositoryBase<TalentModel, TalentEntity>, ITalentRepository
    {
        public TalentRepository(string databaseFolder) : base(databaseFolder, "Imago_Talents.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<TalentEntity>();
        }
    }
}
