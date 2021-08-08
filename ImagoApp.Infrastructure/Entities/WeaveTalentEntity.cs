using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class WeaveTalentEntity : TalentBaseEntity
    {
        public string DifficultyFormula { get; set; }
        public string WeaveSource { get; set; }
        
        public List<SkillRequirementEntity> Requirements { get; set; }

        public string RangeFormula { get; set; }

        public string CorrosionFormula { get; set; }

        public string DurationFormula { get; set; }
        public string StrengthOfTalentDescription { get; set; }
    }
}
