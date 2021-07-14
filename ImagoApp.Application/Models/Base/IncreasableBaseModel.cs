using ImagoApp.Application.Constants;
using Newtonsoft.Json;

namespace ImagoApp.Application.Models.Base
{
    public abstract class IncreasableBaseModel : ModifiableBaseModel
    {
        private int _totalExperience;

        public int TotalExperience
        {
            get => _totalExperience;
            set
            {
                SetProperty(ref _totalExperience, value);
                (int IncreaseLevel, int LeftoverExperience, int ExperienceForNextIncrease) increaseInfo = (0,0,0);

                if (this is AttributeModel attribute)
                    increaseInfo = IncreaseConstants.GetIncreaseInfo(IncreaseConstants.IncreaseType.Attribute, value + attribute.ExperienceBySkillGroup);
                if (this is SkillGroupModel)
                    increaseInfo = IncreaseConstants.GetIncreaseInfo(IncreaseConstants.IncreaseType.SkillGroup, value);
                if (this is SkillModel) 
                    increaseInfo = IncreaseConstants.GetIncreaseInfo(IncreaseConstants.IncreaseType.Skill, value);

                ExperienceValue = increaseInfo.LeftoverExperience;
                IncreaseValue = increaseInfo.IncreaseLevel;
                ExperienceForNextIncreasedRequired = increaseInfo.ExperienceForNextIncrease;

                OnPropertyChanged(nameof(IncreaseValue));
                OnPropertyChanged(nameof(ExperienceValue));
                OnPropertyChanged(nameof(ExperienceForNextIncreasedRequired));

                if (this is AttributeModel attribute2)
                    attribute2.RecalculateFinalValue();
                if (this is SkillGroupModel skillGroup)
                    skillGroup.RecalculateFinalValue();
                if (this is SkillModel skill)
                    skill.RecalculateFinalValue();

                OnPropertyChanged(nameof(FinalValue));
            }
        }
        
        public int ExperienceValue { get; private set; }
        public int IncreaseValue { get; private set; }
        public int ExperienceForNextIncreasedRequired { get; private set; }
    }
}