using System;
using System.Collections.Generic;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class MasteryModel : TalentBase
    {
        private List<(SkillGroupModelType, int)> _requirements;
        private SkillGroupModelType _targetSkill;


        public MasteryModel() : base()
        {

        }

        public MasteryModel(SkillGroupModelType targetSkill , string name, string shortDescription,string description, List<(SkillGroupModelType, int)> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription, description, activeUse, difficulty, phaseValueMod)
        {
            TargetSkill = targetSkill;
            Requirements = requirements;
        }

        public SkillGroupModelType TargetSkill
        {
            get => _targetSkill;
            set => SetProperty(ref _targetSkill, value);
        }

        public List<(SkillGroupModelType, int)> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements , value);
        }

     
    }
}
