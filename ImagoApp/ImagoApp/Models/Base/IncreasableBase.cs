using Newtonsoft.Json;

namespace ImagoApp.Models.Base
{
    public abstract class IncreasableBase : ModifiableBase
    {
        private int _totalExperience;

        public int TotalExperience
        {
            get => _totalExperience;
            set
            {
                SetProperty(ref _totalExperience, value);
                (int IncreaseLevel, int LeftoverExperience, int ExperienceForNextIncrease) increaseInfo = (0,0,0);

                if (this is Attribute attribute)
                    increaseInfo = Services.IncreaseService.GetIncreaseInfo(Services.IncreaseType.Attribute, value + attribute.ExperienceBySkillGroup);
                if (this is SkillGroupModel)
                    increaseInfo = Services.IncreaseService.GetIncreaseInfo(Services.IncreaseType.SkillGroup, value);
                if (this is SkillModel) 
                    increaseInfo = Services.IncreaseService.GetIncreaseInfo(Services.IncreaseType.Skill, value);

                ExperienceValue = increaseInfo.LeftoverExperience;
                IncreaseValue = increaseInfo.IncreaseLevel;
                ExperienceForNextIncreasedRequired = increaseInfo.ExperienceForNextIncrease;

                OnPropertyChanged(nameof(IncreaseValue));
                OnPropertyChanged(nameof(ExperienceValue));
                OnPropertyChanged(nameof(ExperienceForNextIncreasedRequired));

                if (this is Attribute attribute2)
                    Util.SkillHelper.RecalculateFinalValue(attribute2);
                if (this is SkillGroupModel skillGroup) 
                    Util.SkillHelper.RecalculateFinalValue(skillGroup);
                if (this is SkillModel skill) 
                    Util.SkillHelper.RecalculateFinalValue(skill);

                OnPropertyChanged(nameof(FinalValue));
            }
        }

        [JsonIgnore]
        public int ExperienceValue { get; private set; }
        [JsonIgnore]
        public int IncreaseValue { get; private set; }
        [JsonIgnore]
        public int ExperienceForNextIncreasedRequired { get; private set; }
    }
}