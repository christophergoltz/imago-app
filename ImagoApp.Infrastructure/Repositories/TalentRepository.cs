using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Infrastructure.Entities;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface ITalentRepository : IRepository<TalentEntity>
    {
    }

    public class TalentRepository : Repository<TalentEntity>, ITalentRepository
    {
        public TalentRepository(string databaseFolder) : base(databaseFolder)
        {
        }
    }
}
