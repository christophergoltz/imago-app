using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Services
{
    public interface IAttributeCalculationService
    {
        //attributes
        /// <summary>
        /// Apply the given creation experience value to the attribute
        /// </summary>
        /// <param name="characterModel"></param>
        /// <param name="target"></param>
        /// <param name="creationExperience"></param>
        /// <returns>Returns if the attribute final value has changed</returns>
        bool SetCreationExperience(CharacterModel characterModel, AttributeModel target, int creationExperience);

        /// <summary>
        /// Apply the given experience value to the attribute
        /// </summary>
        /// <param name="characterModel"></param>
        /// <param name="target"></param>
        /// <param name="experience"></param>
        /// <returns>Returns if the attribute final value has changed</returns>
        bool SetExperience(CharacterModel characterModel, AttributeModel target, int experience);

        /// <summary>
        /// Changes the given skillgroup experience value to the attribute
        /// </summary>
        /// <param name="characterModel"></param>
        /// <param name="target"></param>
        /// <param name="skillGroupExperienceChange"></param>
        /// <returns>Returns if the attribute final value has changed</returns>
        bool AddSkillGroupExperience(CharacterModel characterModel, AttributeModel target, int skillGroupExperienceChange);

        bool SetCorrosion(CharacterModel characterModel, AttributeModel target, int corrosion);
        bool SetModification(CharacterModel characterModel, AttributeModel target, int modification);
        bool SetBaseValue(CharacterModel characterModel, AttributeModel target, int baseValue);

        //skill
        bool SetModification(CharacterModel characterModel, SkillModel target, int modification);
        (bool FinalValueChanged, int IncreaseValueChange) AddExperience(CharacterModel characterModel, SkillModel target, int experience);

        //skillgroup
        bool SetModification(CharacterModel characterModel, SkillGroupModel target, int modification);
        (bool FinalValueChanged, int IncreaseValueChange) AddExperience(CharacterModel characterModel, SkillGroupModel target, int experience);
        (bool FinalValueChanged, int IncreaseValueChange) SetCreationExperience(CharacterModel characterModel, SkillModel target, int creationExperience);
        void RecalculateAllAttributes(CharacterModel characterModel);
    }

    public class AttributeCalculationService : IAttributeCalculationService
    {
        public bool SetCreationExperience(CharacterModel characterModel, AttributeModel target, int creationExperience)
        {
            target.CreationExperience = creationExperience;

            //recalculate increase info
            var increaseValueChanged = RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return false;

            return RecalculateFinalValue(characterModel, target);
        }

        public bool SetCorrosion(CharacterModel characterModel, AttributeModel target, int corrosion)
        {
            target.Corrosion = corrosion;
            return RecalculateFinalValue(characterModel, target);
        }

        public bool SetBaseValue(CharacterModel characterModel, AttributeModel target, int baseValue)
        {
            target.BaseValue = baseValue;
            return RecalculateFinalValue(characterModel, target);
        }

        public (bool FinalValueChanged, int IncreaseValueChange) SetCreationExperience(CharacterModel characterModel, SkillModel target, int creationExperience)
        {
            target.CreationExperience = creationExperience;

            //recalculate increase info
            var increaseValueChanged = RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return (false, 0);

            //recalculate final value
            var finalValueChanged = RecalculateFinalValue(target);
            return (finalValueChanged, increaseValueChanged);
        }

        public (bool FinalValueChanged, int IncreaseValueChange) AddExperience(CharacterModel characterModel, SkillModel target, int experience)
        {
            target.ExperienceValue += experience;

            //recalculate increase info
            var increaseValueChanged = RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return (false, 0);

            //recalculate final value
            var finalValueChanged = RecalculateFinalValue(target);
            return (finalValueChanged, increaseValueChanged);
        }

        public bool SetModification(CharacterModel characterModel, SkillModel target, int modification)
        {
            target.ModificationValue = modification;
            var finalValueChanged = RecalculateFinalValue(target);
            return finalValueChanged;
        }

        public bool SetModification(CharacterModel characterModel, SkillGroupModel target, int modification)
        {
            target.ModificationValue = modification;
            var finalValueChanged = RecalculateFinalValue(characterModel, target);
            return finalValueChanged;
        }

        public bool SetModification(CharacterModel characterModel, AttributeModel target, int modification)
        {
            target.ModificationValue = modification;
            return RecalculateFinalValue(characterModel, target);
        }

        public (bool FinalValueChanged, int IncreaseValueChange) AddExperience(CharacterModel characterModel, SkillGroupModel target, int experience)
        {
            target.ExperienceValue += experience;

            //recalculate increase info
            var increaseValueChanged = RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return (false, 0);

            var finalValueChanged = RecalculateFinalValue(characterModel, target);
            
            return (finalValueChanged, increaseValueChanged);
        }

        public bool SetExperience(CharacterModel characterModel, AttributeModel target, int experience)
        {
            target.ExperienceValue = experience;

            //recalculate increase info
            var increaseValueChanged = RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return false;
            
            return RecalculateFinalValue(characterModel, target);
        }
        
        public bool AddSkillGroupExperience(CharacterModel characterModel, AttributeModel target, int skillGroupExperienceChange)
        {
            target.ExperienceBySkillGroup += skillGroupExperienceChange;

            //recalculate increase info
            var increaseValueChanged = RecalculateIncreaseInfo(target);
            if (increaseValueChanged == 0)
                return false;

            //recalculate final value
            return RecalculateFinalValue(characterModel, target);
        }
        
        /// <summary>
        /// Recalculates the increase infos
        /// </summary>
        /// <param name="increasableBase"></param>
        /// <returns>Returns the increase value change</returns>
        private int RecalculateIncreaseInfo(IncreasableBaseModel increasableBase)
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

        public void RecalculateAllAttributes(CharacterModel characterModel)
        {
            foreach (var skillGroup in characterModel.SkillGroups)
            {
                RecalculateIncreaseInfo(skillGroup);

                foreach (var skill in skillGroup.Skills)
                {
                    RecalculateIncreaseInfo(skill);
                }
            }

            foreach (var attribute in characterModel.Attributes)
            {
                RecalculateIncreaseInfo(attribute);
                RecalculateFinalValue(characterModel, attribute);
            }
        }

        private bool RecalculateFinalValue(SkillModel skillModel)
        {
            var oldFinalValue = skillModel.FinalValue;
            skillModel.FinalValue = skillModel.BaseValue + skillModel.IncreaseValue + skillModel.ModificationValue;

            return oldFinalValue != skillModel.FinalValue;

            //todo update masteries and talents
        }

        private bool RecalculateFinalValue(CharacterModel characterModel, SkillGroupModel skillgroup)
        {
            var oldFinalValue = skillgroup.FinalValue;
            skillgroup.FinalValue = skillgroup.BaseValue + skillgroup.IncreaseValue + skillgroup.ModificationValue;
           
            var finalValueChanged = oldFinalValue != skillgroup.FinalValue;
            if(finalValueChanged)
            {
                //update dependent items
                ApplyNewFinalValueOfSkillgroup(characterModel, skillgroup);
            }
            return finalValueChanged;
        }

        private bool RecalculateFinalValue(CharacterModel characterModel, AttributeModel target)
        {
            var oldFinalValue = target.FinalValue;
            target.FinalValue = target.BaseValue + target.IncreaseValue + target.ModificationValue - target.Corrosion;

            var finalValueChanged = oldFinalValue != target.FinalValue;
            if (finalValueChanged)
            {
                //update dependent items
                ApplyNewFinalValueOfAttribute(characterModel, target);
            }

            return finalValueChanged;
        }

        private void UpdateNewBaseValueToSkillsOfGroup(SkillGroupModel skillgroup)
        {
            foreach (var skill in skillgroup.Skills)
            {
                skill.BaseValue = (int)skillgroup.FinalValue;
                RecalculateFinalValue(skill);
            }
        }

        private void ApplyNewFinalValueOfSkillgroup(CharacterModel characterModel, SkillGroupModel skillgroup)
        {
            foreach (var skill in skillgroup.Skills)
            {
                skill.BaseValue = (int) skillgroup.FinalValue;
                RecalculateFinalValue(skill);
            }
        }

        private void ApplyNewFinalValueOfAttribute(CharacterModel characterModel, AttributeModel changedAttribute)
        {
            //updating all dependent skillgroups
            var affectedSkillGroupTypes = RuleConstants.GetSkillGroupsByAttribute(changedAttribute.Type);
            var skillGroups = characterModel.SkillGroups
                .Where(model => affectedSkillGroupTypes
                    .Contains(model.Type));

            foreach (var affectedSkillGroup in skillGroups)
            {
                var attributeTypesForCalculation = RuleConstants.GetSkillGroupSources(affectedSkillGroup.Type);

                double tmp = 0;
                foreach (var attributeType in attributeTypesForCalculation)
                {
                    tmp += characterModel.Attributes.First(attribute => attribute.Type == attributeType).FinalValue;
                }

                var newBaseValue = (int)Math.Round((tmp / 6), MidpointRounding.AwayFromZero);

                affectedSkillGroup.BaseValue = newBaseValue;
                RecalculateFinalValue(characterModel, affectedSkillGroup);

                UpdateNewBaseValueToSkillsOfGroup(affectedSkillGroup);
            }
        }
    }
}
