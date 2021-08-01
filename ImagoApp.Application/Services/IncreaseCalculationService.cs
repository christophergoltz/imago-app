﻿using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;

namespace ImagoApp.Application.Services
{
    public interface IIncreaseCalculationService
    {
        /// <summary>
        /// Recalculates the increase infos
        /// </summary>
        /// <param name="increasableBase"></param>
        /// <returns>Returns the increase value change</returns>
        int RecalculateIncreaseInfo(IncreasableBaseModel increasableBase);
    }

    public class IncreaseCalculationService : IIncreaseCalculationService
    {
        private static readonly int[] ExperienceLookup = { 2, 3, 5, 8, 12, 17, 23, 30, 38, 47 };

        public enum IncreaseType
        {
            Unknown = 0,
            Attribute,
            SkillGroup,
            Skill
        }

        /// <summary>
        /// Recalculates the increase infos
        /// </summary>
        /// <param name="increasableBase"></param>
        /// <returns>Returns the increase value change</returns>
        public int RecalculateIncreaseInfo(IncreasableBaseModel increasableBase)
        {
            var totalExperience = increasableBase.ExperienceValue;
            var increaseType = IncreaseType.Unknown;

            if (increasableBase is AttributeModel attributeModel)
            {
                totalExperience += attributeModel.CreationExperience + attributeModel.ExperienceBySkillGroup;
                increaseType = IncreaseType.Attribute;
            }
            if (increasableBase is SkillModel skillModel)
            {
                totalExperience += skillModel.CreationExperience;
                increaseType = IncreaseType.Skill;
            }
            if (increasableBase is SkillGroupModel)
            {
                increaseType = IncreaseType.SkillGroup;
            }

            var info = GetIncreaseInfo(increaseType, totalExperience);

            var oldIncreaseValue = increasableBase.IncreaseValueCache;
            increasableBase.IncreaseValueCache = info.IncreaseLevel;
            increasableBase.ExperienceForNextIncreasedRequiredCache = info.ExperienceForNextIncrease;
            increasableBase.LeftoverExperienceCache = info.LeftoverExperience;

            return info.IncreaseLevel - oldIncreaseValue;
        }

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

        public static int GetExperienceRequiredForLevel(IncreaseType increaseType, int increaseValue)
        {
            var currentIncrease = 0;
            var requiredExperience = 0;
            while (currentIncrease < increaseValue)
            {
                var costForNextIncrease = GetExperienceRequiredForNextIncrease(increaseType, currentIncrease);
                requiredExperience += costForNextIncrease;
                currentIncrease++;
            }

            return requiredExperience;
        }

        private static (int IncreaseLevel, int LeftoverExperience, int ExperienceForNextIncrease) GetIncreaseInfo(IncreaseType increaseType, int totalExperience)
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

            return (increaseValue, leftoverExperience, costForNextIncrease);
        }
    }
}
