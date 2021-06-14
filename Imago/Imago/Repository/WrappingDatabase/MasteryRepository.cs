using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imago.Models;
using Imago.Models.Entity;

namespace Imago.Repository.WrappingDatabase
{
    public interface IMasteryRepository : IObjectJsonRepository<MasteryModel>
    {
        Task EnsureTables();
    }

    public class MasteryRepository : ObjectJsonRepositoryBase<MasteryModel, MasteryEntity>, IMasteryRepository
    {
        public MasteryRepository(string databaseFolder) : base(databaseFolder, "Imago_Mastery.db3") { }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<MasteryEntity>();
        }
    }
}
