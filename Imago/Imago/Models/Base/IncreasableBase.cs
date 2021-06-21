using System;
using System.Collections.Generic;
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

                if (this is Attribute)
                    increaseInfo = IncreaseServices.GetIncreaseInfo(IncreaseType.Attribute, value);
                if (this is SkillGroup)
                    increaseInfo = IncreaseServices.GetIncreaseInfo(IncreaseType.Attribute, value);
                if (this is Skill) 
                    increaseInfo = IncreaseServices.GetIncreaseInfo(IncreaseType.Attribute, value);

                ExperienceValue = increaseInfo.LeftoverExperience;
                IncreaseValue = increaseInfo.IncreaseLevel;
                ExperienceForNextIncreasedRequired = increaseInfo.ExperienceForNextIncrease;

                OnPropertyChanged(nameof(IncreaseValue));
                OnPropertyChanged(nameof(ExperienceValue));
                OnPropertyChanged(nameof(ExperienceForNextIncreasedRequired));
            }
        }

        public int ExperienceValue { get; private set; }
        public int IncreaseValue { get; private set; }
        public int ExperienceForNextIncreasedRequired { get; private set; }
    }
}