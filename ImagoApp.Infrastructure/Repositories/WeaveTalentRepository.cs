using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Infrastructure.Entities;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IWeaveTalentRepository : IRepository<WeaveTalentEntity>
    {
    }

    public class WeaveTalentRepository : Repository<WeaveTalentEntity>, IWeaveTalentRepository
    {
        public WeaveTalentRepository(string databaseFile) : base(databaseFile)
        {
        }
    }
}
