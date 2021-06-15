using System;
using System.Collections.Generic;
using System.Text;
using Imago.Converter;
using Imago.Util;

namespace Imago.Models
{
    public class TalentModel : TalentBase
    {
        private Dictionary<SkillType, int> _requirements;
        private SkillType _targetSkill;

        public TalentModel() : base()
        {
            
        }

        public TalentModel(SkillType targetSkill, string name,string shortDescription, string description, Dictionary<SkillType, int> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription,description, activeUse, difficulty, phaseValueMod)
        {
            TargetSkill = targetSkill;
            Requirements = requirements;
        }

        public SkillType TargetSkill
        {
            get => _targetSkill;
            set => SetProperty(ref _targetSkill, value);
        }

        public Dictionary<SkillType, int> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements, value);
        }
    }
}
