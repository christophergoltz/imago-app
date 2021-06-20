using System;
using System.Collections.Generic;
using System.Text;
using Imago.Repository;
using Imago.Util;
using Newtonsoft.Json;

namespace Imago.Models.Base
{
    public abstract class IncreasableBase : ModifiableBase
    {
        private int _experienceValue;
        private int _increaseValue;

        public int ExperienceValue
        {
            get => _experienceValue;
            set => SetProperty(ref _experienceValue, value);
        }

        public int IncreaseValue
        {
            get => _increaseValue;
            set
            {
                SetProperty(ref _increaseValue, value);
                OnPropertyChanged(nameof(ExperienceForNextIncrease));
            }
        }

        [JsonIgnore]
        public int ExperienceForNextIncrease => SkillIncreaseHelper.GetExperienceForNextSkillBaseLevel(this);
    }
}