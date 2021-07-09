using System.Collections.Generic;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class MasteryEntity  : TalentBaseEntity
    {
        public SkillGroupModelType TargetSkill { get; set; }

        public List<SkillGroupRequirementEntity> Requirements { get; set; }
    }
}
