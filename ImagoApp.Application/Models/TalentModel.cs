﻿using System.Collections.Generic;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class TalentModel : TalentBase
    {
        private List<SkillRequirementModel> _requirements;
        private SkillModelType _targetSkillModel;

        public TalentModel() : base()
        {
            
        }

        public TalentModel(SkillModelType targetSkillModel, string name,string shortDescription, string description, List<SkillRequirementModel> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription,description, activeUse, difficulty, phaseValueMod)
        {
            TargetSkillModel = targetSkillModel;
            Requirements = requirements;
        }

        public SkillModelType TargetSkillModel
        {
            get => _targetSkillModel;
            set => SetProperty(ref _targetSkillModel, value);
        }

        public List<SkillRequirementModel> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements, value);
        }
    }
}