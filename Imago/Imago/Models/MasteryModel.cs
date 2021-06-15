using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class MasteryModel : TalentBase
    {
        private Dictionary<SkillGroupType, int> _requirements;
        private SkillGroupType _targetSkill;


        public MasteryModel() : base()
        {

        }

        public MasteryModel(SkillGroupType targetSkill , string name, string shortDescription,string description, Dictionary<SkillGroupType, int> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription, description, activeUse, difficulty, phaseValueMod)
        {
            TargetSkill = targetSkill;
            Requirements = requirements;
        }

        public SkillGroupType TargetSkill
        {
            get => _targetSkill;
            set => SetProperty(ref _targetSkill, value);
        }

        public Dictionary<SkillGroupType, int> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements , value);
        }

     
    }
}
