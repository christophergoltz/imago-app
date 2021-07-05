using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Infrastructure.Entities;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IMasteryRepository : IRepository<MasteryEntity>
    {
    }

    public class MasteryRepository : Repository<MasteryEntity>, IMasteryRepository
    {
        public MasteryRepository(string databaseFolder) : base(databaseFolder)
        {
        }
    }
}