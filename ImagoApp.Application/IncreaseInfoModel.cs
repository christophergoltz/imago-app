using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Application
{
    public class IncreaseInfoModel
    {
        public readonly int TotalExperience;
        public readonly int IncreaseLevel;
        public readonly int LeftoverExperience;
        public readonly int ExperienceForNextIncrease;

        public IncreaseInfoModel(int totalExperience, int increaseLevel, int leftoverExperience, int experienceForNextIncrease)
        {
            TotalExperience = totalExperience;
            IncreaseLevel = increaseLevel;
            LeftoverExperience = leftoverExperience;
            ExperienceForNextIncrease = experienceForNextIncrease;
        }
    }
}
