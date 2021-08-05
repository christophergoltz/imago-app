﻿using System.Collections.Generic;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class TalentModel : TalentBaseModel
    {
        private List<SkillRequirementModel> _requirements;
        private SkillModelType _targetSkillModel;
        private string _phaseValueMod;

        public TalentModel() : base()
        {

        }

        public TalentModel(SkillModelType targetSkillModel, string name, string shortDescription, string description, List<SkillRequirementModel> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription, description, activeUse, difficulty)
        {
            TargetSkillModel = targetSkillModel;
            Requirements = requirements;
            PhaseValueMod = phaseValueMod;
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

        public string PhaseValueMod
        {
            get => _phaseValueMod;
            set => SetProperty(ref _phaseValueMod, value);
        }
    }
}
