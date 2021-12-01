using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Services
{
    public static class IncreaseCalculationService
    {
        private static readonly int[] ExperienceLookup = { 2, 3, 5, 8, 12, 17, 23, 30, 38, 47 };
        
    
        private static int GetExperienceRequiredForNextIncrease(IncreaseType increaseType, int increaseValue)
        {
            var index = 0;

            switch (increaseType)
            {
                case IncreaseType.Attribute:
                    {
                        var indexPart = (double)increaseValue / 10;
                        index = (int)Math.Floor(indexPart) - 2;
                        break;
                    }
                case IncreaseType.SkillGroup:
                    {
                        var indexPart = (double)increaseValue / 5;
                        index = (int)Math.Floor(indexPart) + 2;
                        break;
                    }
                case IncreaseType.Skill:
                    {
                        var indexPart = (double)increaseValue / 15;
                        index = (int)Math.Floor(indexPart);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(increaseType));
            }

            if (index < 0)
                index = 0;

            if (index > 9)
                index = 9;

            return ExperienceLookup[index];
        }
        
        public static IncreaseInfoModel GetIncreaseInfo(IncreaseType increaseType, int totalExperience)
        {
            var leftoverExperience = totalExperience;
            var increaseValue = 0;
            int costForNextIncrease;

            while (true)
            {
                costForNextIncrease = GetExperienceRequiredForNextIncrease(increaseType, increaseValue);

                if (leftoverExperience < costForNextIncrease)
                    break; //cant increase

                //increase
                leftoverExperience -= costForNextIncrease;
                increaseValue++;
            }

            return new IncreaseInfoModel(totalExperience, increaseValue, leftoverExperience, costForNextIncrease);
        }
    }
}
