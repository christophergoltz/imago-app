using System;
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
        /// <summary>
        /// Recalculates the increase infos
        /// </summary>
        /// <param name="increasableBase"></param>
        /// <returns>Returns the increase value change</returns>
        public int RecalculateIncreaseInfo(IncreasableBaseModel increasableBase)
        {
            var totalExperience = increasableBase.ExperienceValue;

            if (increasableBase is AttributeModel attributeModel)
                totalExperience += attributeModel.CreationExperience + attributeModel.ExperienceBySkillGroup;
            if (increasableBase is SkillModel skillModel)
                totalExperience += skillModel.CreationExperience;

            var info = IncreaseConstants.GetIncreaseInfo(IncreaseConstants.IncreaseType.Attribute, totalExperience);

            var oldIncreaseValue = increasableBase.IncreaseValue;
            increasableBase.IncreaseValue = info.IncreaseLevel;
            increasableBase.ExperienceForNextIncreasedRequired = info.ExperienceForNextIncrease;
            increasableBase.LeftoverExperience = info.LeftoverExperience;

            return info.IncreaseLevel - oldIncreaseValue;
        }

    }
}
