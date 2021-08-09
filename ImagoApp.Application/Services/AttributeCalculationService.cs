using System;
using System.Collections.Generic;
using System.Linq;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;

namespace ImagoApp.Application.Services
{
    public interface IAttributeCalculationService
    {
        bool SetCreationExperience(AttributeModel target, int creationExperience, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups);
        bool SetCorrosion(AttributeModel target, int corrosion, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups);
        bool SetBaseValue(AttributeModel target, int baseValue, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups);
        bool SetModification(AttributeModel target, int modification, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups);
        bool SetExperience(AttributeModel target, int experience, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups);
        bool AddSkillGroupExperience(AttributeModel target, int skillGroupExperienceChange, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups);
        void RecalculateAllAttributes(List<AttributeModel> attributes, List<SkillGroupModel> skillGroups);
    }

    public class AttributeCalculationService : IAttributeCalculationService
    {
        private readonly IIncreaseCalculationService _increaseCalculationService;
        private readonly ISkillGroupCalculationService _skillGroupCalculationService;

        public AttributeCalculationService(IIncreaseCalculationService increaseCalculationService, ISkillGroupCalculationService skillGroupCalculationService)
        {
            _increaseCalculationService = increaseCalculationService;
            _skillGroupCalculationService = skillGroupCalculationService;
        }

        public bool SetCreationExperience(AttributeModel target, int creationExperience, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            target.CreationExperience = creationExperience;

            //recalculate increase info
            var increaseValueChanged = _increaseCalculationService.RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return false;

            return RecalculateFinalValue(target, attributes, skillGroups);
        }

        public bool SetCorrosion(AttributeModel target, int corrosion, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            target.Corrosion = corrosion;
            return RecalculateFinalValue(target, attributes, skillGroups);
        }

        public bool SetBaseValue(AttributeModel target, int baseValue, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            target.BaseValue = baseValue;
            return RecalculateFinalValue(target, attributes, skillGroups);
        }

        public bool SetModification(AttributeModel target, int modification, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            target.ModificationValue = modification;
            return RecalculateFinalValue(target, attributes, skillGroups);
        }
    
        public bool SetExperience(AttributeModel target, int experience, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            target.ExperienceValue = experience;

            //recalculate increase info
            var increaseValueChanged = _increaseCalculationService.RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return false;
            
            return RecalculateFinalValue(target, attributes, skillGroups);
        }
        
        public bool AddSkillGroupExperience(AttributeModel target, int skillGroupExperienceChange, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            target.ExperienceBySkillGroup += skillGroupExperienceChange;

            //recalculate increase info
            var increaseValueChanged = _increaseCalculationService.RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return false;

            //recalculate final value
            return RecalculateFinalValue(target, attributes, skillGroups);
        }
       
        public void RecalculateAllAttributes(List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            foreach (var skillGroup in skillGroups)
            {
                _increaseCalculationService.RecalculateIncreaseInfo(skillGroup);

                foreach (var skill in skillGroup.Skills)
                {
                    _increaseCalculationService.RecalculateIncreaseInfo(skill);
                }
            }

            foreach (var attribute in attributes)
            {
                _increaseCalculationService.RecalculateIncreaseInfo(attribute);
                RecalculateFinalValue(attribute, attributes, skillGroups);
            }
        }
        
        private bool RecalculateFinalValue(AttributeModel target, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            var oldFinalValue = target.FinalValue;
            target.FinalValue = target.BaseValue + target.IncreaseValueCache + target.ModificationValue - target.Corrosion;

            var finalValueChanged = oldFinalValue != target.FinalValue;
            if (finalValueChanged)
            {
                //update dependent items
                ApplyNewFinalValueOfAttribute(target, attributes, skillGroups);
            }

            return finalValueChanged;
        }

        private void ApplyNewFinalValueOfAttribute(AttributeModel changedAttribute, List<AttributeModel> attributes, List<SkillGroupModel> skillGroups)
        {
            //updating all dependent skillgroups
            var affectedSkillGroupTypes = RuleConstants.GetSkillGroupsByAttribute(changedAttribute.Type);

            foreach (var affectedSkillGroup in skillGroups.Where(model => affectedSkillGroupTypes.Contains(model.Type)))
            {
                var attributeTypesForCalculation = RuleConstants.GetSkillGroupSources(affectedSkillGroup.Type);

                var attributeSum = attributeTypesForCalculation.Sum(attributeType => attributes.First(attribute => attribute.Type == attributeType).FinalValue);
                var newBaseValue = attributeSum / 6;

                affectedSkillGroup.BaseValue = newBaseValue.GetRoundedValue();
                _skillGroupCalculationService.RecalculateFinalValue(affectedSkillGroup);
                _skillGroupCalculationService.UpdateNewBaseValueToSkillsOfGroup(affectedSkillGroup);
            }
        }
    }
}
