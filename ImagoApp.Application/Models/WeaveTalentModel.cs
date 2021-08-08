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
        private string _rangeFormula;
        private string _corrosionFormula;
        private string _durationFormula;
        private string _weaveSource;
        private string _difficultyFormula;
        private string _strengthOfTalentDescription;

        public WeaveTalentModel() : base()
        {
            
        }

        public WeaveTalentModel(string weaveSource, List<SkillRequirementModel> requirements, string name, string shortDescription, string description,
          bool activeUse, string difficultyFormula, string rangeFormula, string corrosionFormula, string durationFormula, string strengthOfTalentDescription) : base(name, shortDescription, description,  activeUse)
        {
            WeaveSource = weaveSource;
            Requirements = requirements;
            RangeFormula = rangeFormula;
            CorrosionFormula = corrosionFormula;
            DurationFormula = durationFormula;
            StrengthOfTalentDescription = strengthOfTalentDescription;
        }

        public string WeaveSource
        {
            get => _weaveSource;
            set => SetProperty(ref _weaveSource, value);
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

        public string DifficultyFormula
        {
            get => _difficultyFormula;
            set => SetProperty(ref _difficultyFormula, value);
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

        public string StrengthOfTalentDescription
        {
            get => _strengthOfTalentDescription;
            set => SetProperty(ref _strengthOfTalentDescription, value);
        }
    }
}
