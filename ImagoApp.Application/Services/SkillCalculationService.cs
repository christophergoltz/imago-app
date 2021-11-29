using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application.Models;

namespace ImagoApp.Application.Services
{
    public interface ISkillCalculationService
    {
        bool SetModification(SkillModel target, int modification);
        (bool FinalValueChanged, int IncreaseValueChange) SetCreationExperience(SkillModel target, int creationExperience);
        (bool FinalValueChanged, int IncreaseValueChange) AddExperience(SkillModel target, int experience);
        bool RecalculateFinalValue(SkillModel skillModel);
    }

    public class SkillCalculationService : ISkillCalculationService
    {
        public bool RecalculateFinalValue(SkillModel skillModel)
        {
            var oldFinalValue = skillModel.FinalValue;
            skillModel.FinalValue = skillModel.BaseValue + skillModel.IncreaseValueCache + skillModel.ModificationValue;

            return oldFinalValue != skillModel.FinalValue;

            //todo update masteries and talents
        }

        public bool SetModification(SkillModel target, int modification)
        {
            target.ModificationValue = modification;
            var finalValueChanged = RecalculateFinalValue(target);
            return finalValueChanged;
        }


        public (bool FinalValueChanged, int IncreaseValueChange) SetCreationExperience(SkillModel target, int creationExperience)
        {
            var oldIncreaseValue = target.IncreaseValueCache;
            target.CreationExperience = creationExperience;
            var newIncreaseValue = target.IncreaseValueCache;
            
            //recalculate increase info
            var increaseValueChanged = newIncreaseValue - oldIncreaseValue;
            if (increaseValueChanged == 0)
                return (false, 0);

            //recalculate final value
            var finalValueChanged = RecalculateFinalValue(target);
            return (finalValueChanged, increaseValueChanged);
        }

        public (bool FinalValueChanged, int IncreaseValueChange) AddExperience(SkillModel target, int experience)
        {
            var oldIncreaseValue = target.IncreaseValueCache;
            target.ExperienceValue += experience;
            var newIncreaseValue = target.IncreaseValueCache;

            //recalculate increase info
            var increaseValueChanged = newIncreaseValue - oldIncreaseValue;
            if (increaseValueChanged == 0)
                return (false, 0);

            //recalculate final value
            var finalValueChanged = RecalculateFinalValue(target);
            return (finalValueChanged, increaseValueChanged);
        }

    }
}
