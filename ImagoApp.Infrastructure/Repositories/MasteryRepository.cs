using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Shared.Enums;
using LiteDB;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IMasteryRepository : IRepository<MasteryEntity>
    {
        List<MasteryEntity> GetAllMasteries(SkillGroupModelType groupModelType);
    }

    public class MasteryRepository : Repository<MasteryEntity>, IMasteryRepository
    {
        public MasteryRepository(string databaseFile) : base(databaseFile)
        {
        }

        public List<MasteryEntity> GetAllMasteries(SkillGroupModelType groupModelType)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var collection = db.GetCollection<MasteryEntity>(nameof(MasteryEntity));
                return collection.Query().Where(entity => entity.TargetSkill == groupModelType).ToList();
            }
        }
    }
}