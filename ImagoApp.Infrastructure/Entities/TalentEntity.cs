using System.Collections.Generic;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class TalentEntity : TalentBaseEntity
    {
        public SkillModelType TargetSkillModel { get; set; }

        public List<SkillRequirementEntity> Requirements { get; set; }

        public string PhaseValueMod { get; set; }
    }
}
