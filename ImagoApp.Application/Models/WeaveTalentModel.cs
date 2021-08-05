using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class WeaveTalentModel : TalentBaseModel
    {
        private List<SkillRequirementModel> _requirements;
        private SkillModelType _targetSkillModel;
        private string _rangeFormula;
        private string _corrosionFormula;
        private string _durationFormula;

        public WeaveTalentModel() : base()
        {
            
        }

        public WeaveTalentModel(SkillModelType targetSkillModel, List<SkillRequirementModel> requirements, string name, string shortDescription, string description,
          bool activeUse, int? difficulty, string rangeFormula, string corrosionFormula, string durationFormula  ) : base(name, shortDescription, description,  activeUse,difficulty )
        {
            TargetSkillModel = targetSkillModel;
            Requirements = requirements;
            RangeFormula = rangeFormula;
            CorrosionFormula = corrosionFormula;
            DurationFormula = durationFormula;
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

        public string RangeFormula
        {
            get => _rangeFormula;
            set => SetProperty(ref _rangeFormula, value);
        }

        public string CorrosionFormula
        {
            get => _corrosionFormula;
            set => SetProperty(ref _corrosionFormula, value);
        }

        public string DurationFormula
        {
            get => _durationFormula;
            set => SetProperty(ref _durationFormula , value);
        }
    }
}
