using System;
using System.Collections.Generic;
using System.Text;
using Imago.Converter;
using Imago.Util;

namespace Imago.Models
{
    public class TalentModel : TalentBase
    {
        private Dictionary<SkillModelType, int> _requirements;
        private SkillModelType _targetSkillModel;

        public TalentModel() : base()
        {
            
        }

        public TalentModel(SkillModelType targetSkillModel, string name,string shortDescription, string description, Dictionary<SkillModelType, int> requirements,
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

        public Dictionary<SkillModelType, int> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements, value);
        }
    }
}
