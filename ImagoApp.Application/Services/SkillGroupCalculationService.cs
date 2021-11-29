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
        bool RecalculateFinalValue(SkillGroupModel skillgroup, bool skipChange = false);
    }

    public class SkillGroupCalculationService : ISkillGroupCalculationService
    {
        private readonly ISkillCalculationService _skillCalculationService;

        public SkillGroupCalculationService(ISkillCalculationService skillCalculationService)
        {
            _skillCalculationService = skillCalculationService;
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

        public bool RecalculateFinalValue(SkillGroupModel skillgroup, bool skipChange = false)
        {
            var oldFinalValue = skillgroup.FinalValue;
            skillgroup.FinalValue = skillgroup.BaseValue + skillgroup.IncreaseValueCache + skillgroup.ModificationValue;

            var finalValueChanged = oldFinalValue != skillgroup.FinalValue;
            if (finalValueChanged || skipChange)
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
            var oldIncreaseValue = target.IncreaseValueCache;
            target.ExperienceValue += experience;
            var newIncreaseValue = target.IncreaseValueCache;

            //recalculate increase info
            var increaseValueChanged = newIncreaseValue - oldIncreaseValue;
            if (increaseValueChanged == 0)
                return (false, 0);

            var finalValueChanged = RecalculateFinalValue(target);

            return (finalValueChanged, increaseValueChanged);
        }

    }
}
