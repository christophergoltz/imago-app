using System;
using System.Collections.Generic;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class MasteryModel : TalentBaseModel
    {
        private List<SkillGroupRequirementModel> _requirements;
        private SkillGroupModelType _targetSkill;
        private string _phaseValueMod;
        private int? _difficulty;

        public MasteryModel() : base()
        {

        }

        public MasteryModel(SkillGroupModelType targetSkill , string name, string shortDescription,string description, List<SkillGroupRequirementModel> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription, description, activeUse)
        {
            TargetSkill = targetSkill;
            Requirements = requirements;
            PhaseValueMod = phaseValueMod;
            Difficulty = difficulty;
        }

        public SkillGroupModelType TargetSkill
        {
            get => _targetSkill;
            set => SetProperty(ref _targetSkill, value);
        }

        public List<SkillGroupRequirementModel> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements , value);
        }

        public string PhaseValueMod
        {
            get => _phaseValueMod;
            set => SetProperty(ref _phaseValueMod, value);
        }


        public int? Difficulty
        {
            get => _difficulty;
            set => SetProperty(ref _difficulty, value);
        }

    }
}
