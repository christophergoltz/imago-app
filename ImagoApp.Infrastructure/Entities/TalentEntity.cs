using System.Collections.Generic;
using ImagoApp.Shared.Enums;
using SQLite;

namespace ImagoApp.Infrastructure.Entities
{
    public class TalentEntity : TalentBaseEntity
    {
        public SkillModelType TargetSkillModel { get; set; }

        public List<(SkillModelType, int)> Requirements { get; set; }
    }
}
