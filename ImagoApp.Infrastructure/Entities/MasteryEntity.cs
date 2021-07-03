using System.Collections.Generic;
using ImagoApp.Shared.Enums;
using SQLite;

namespace ImagoApp.Infrastructure.Entities
{
    public class MasteryEntity  : TalentBaseEntity
    {
        public SkillGroupModelType TargetSkill { get; set; }

        public List<(SkillGroupModelType, int)> Requirements { get; set; }
    }
}
