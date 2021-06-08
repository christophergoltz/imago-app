using System;
using System.Collections.Generic;
using System.Text;
using Imago.Repository;
using Imago.Util;

namespace Imago.Models.Base
{
    public abstract class SkillBase : DependentBase
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

        public int ExperienceForNextIncrease => SkillIncreaseHelper.GetExperienceForNextSkillBaseLevel(this);
    }
}