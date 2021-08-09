using System.Collections.Generic;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class MasteryEntity  : TalentBaseEntity
    {
        public SkillGroupModelType TargetSkill { get; set; }

        public List<SkillGroupRequirementEntity> Requirements { get; set; }
        public string PhaseValueMod { get; set; }
        public int? Difficulty{ get; set; }
    }
}
