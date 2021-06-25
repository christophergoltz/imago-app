using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Imago.Repository;
using Imago.Services;
using Imago.Util;
using Newtonsoft.Json;

namespace Imago.Models.Base
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
                    increaseInfo = IncreaseService.GetIncreaseInfo(IncreaseType.Attribute, value + attribute.ExperienceBySkillGroup);
                if (this is SkillGroupModel)
                    increaseInfo = IncreaseService.GetIncreaseInfo(IncreaseType.SkillGroup, value);
                if (this is SkillModel) 
                    increaseInfo = IncreaseService.GetIncreaseInfo(IncreaseType.Skill, value);

                ExperienceValue = increaseInfo.LeftoverExperience;
                IncreaseValue = increaseInfo.IncreaseLevel;
                ExperienceForNextIncreasedRequired = increaseInfo.ExperienceForNextIncrease;

                OnPropertyChanged(nameof(IncreaseValue));
                OnPropertyChanged(nameof(ExperienceValue));
                OnPropertyChanged(nameof(ExperienceForNextIncreasedRequired));

                if (this is Attribute attribute2)
                    attribute2.RecalculateFinalValue();
                if (this is SkillGroupModel skillGroup) 
                    skillGroup.RecalculateFinalValue();
                if (this is SkillModel skill) 
                    skill.RecalculateFinalValue();

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