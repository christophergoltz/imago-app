using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application.Models;

namespace ImagoApp.Application.Services
{
    public interface ISkillGroupCalculationService
    {
        void UpdateNewBaseValueToSkillsOfGroup(SkillGroupModel skillGroup);
        bool SetModification(SkillGroupModel target, int modification);
        (bool FinalValueChanged, int IncreaseValueChange) AddExperience(SkillGroupModel target, int experience);
        bool RecalculateFinalValue(SkillGroupModel skillgroup);
    }

    public class SkillGroupCalculationService : ISkillGroupCalculationService
    {
        private readonly ISkillCalculationService _skillCalculationService;
        private readonly IIncreaseCalculationService _increaseCalculationService;

        public SkillGroupCalculationService(ISkillCalculationService skillCalculationService, IIncreaseCalculationService increaseCalculationService)
        {
            _skillCalculationService = skillCalculationService;
            _increaseCalculationService = increaseCalculationService;
        }

        public void UpdateNewBaseValueToSkillsOfGroup(SkillGroupModel skillGroup)
        {
            foreach (var skill in skillGroup.Skills)
            {
                skill.BaseValue = skillGroup.FinalValue.GetRoundedValue();
                _skillCalculationService.RecalculateFinalValue(skill);
            }
        }

        private void ApplyNewFinalValueOfSkillGroup(SkillGroupModel skillGroups)
        {
            foreach (var skill in skillGroups.Skills)
            {
                skill.BaseValue = skillGroups.FinalValue.GetRoundedValue();
                _skillCalculationService.RecalculateFinalValue(skill);
            }
        }

        public bool RecalculateFinalValue(SkillGroupModel skillgroup)
        {
            var oldFinalValue = skillgroup.FinalValue;
            skillgroup.FinalValue = skillgroup.BaseValue + skillgroup.IncreaseValueCache + skillgroup.ModificationValue;

            var finalValueChanged = oldFinalValue != skillgroup.FinalValue;
            if (finalValueChanged)
            {
                //update dependent items
                ApplyNewFinalValueOfSkillGroup(skillgroup);
            }
            return finalValueChanged;
        }

        public bool SetModification(SkillGroupModel target, int modification)
        {
            target.ModificationValue = modification;
            var finalValueChanged = RecalculateFinalValue(target);
            return finalValueChanged;
        }

        public (bool FinalValueChanged, int IncreaseValueChange) AddExperience(SkillGroupModel target, int experience)
        {
            target.ExperienceValue += experience;

            //recalculate increase info
            var increaseValueChanged = _increaseCalculationService.RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return (false, 0);

            var finalValueChanged = RecalculateFinalValue(target);

            return (finalValueChanged, increaseValueChanged);
        }

    }
}
