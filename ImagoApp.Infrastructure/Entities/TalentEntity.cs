using System.Collections.Generic;
using ImagoApp.Shared.Enums;
using SQLite;

namespace ImagoApp.Infrastructure.Entities
{
    public class TalentEntity : TalentBaseEntity
    {
        public SkillModelType TargetSkillModel { get; set; }

        public List<RequirementEntity<SkillModelType>> Requirements { get; set; }
    }
}
