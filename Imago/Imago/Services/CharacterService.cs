using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Base;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Util;
using Attribute = Imago.Models.Attribute;

namespace Imago.Services
{
    public interface ICharacterService
    {
        void SetCorrosionValue(Attribute attribute, int corrosionValue, Character character);
        IEnumerable<SkillGroupType> AddExperienceToSkill(Skill skill, SkillGroup skillGroup, int experience);
        void SetModificationValue(Skill skill, int modificationValue);
        void SetModificationValue(SkillGroup skillGroup, int modificationValue);
        void SetModificationValue(Attribute attribute, int modificationValue, Character character);
        void AddOneExperienceToAttribute(Attribute attribute,List<Attribute> allAttributes, Dictionary<SkillGroupType, SkillGroup> allSkillGroups);
    }

    public class CharacterService : ICharacterService
    {
        private readonly IRuleRepository _ruleRepository;

        public CharacterService(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public void SetModificationValue(Skill skill, int modificationValue)
        {
            skill.ModificationValue = modificationValue;
            skill.RecalculateFinalValue();
        }

        public void SetModificationValue(SkillGroup skillGroup, int modificationValue)
        {
            skillGroup.ModificationValue = modificationValue;
            skillGroup.RecalculateFinalValue();

            UpdateNewBaseValueToSkillsOfGroup(skillGroup);
        }
        
        public void SetModificationValue(Attribute attribute, int modificationValue, Character character)
        {
            attribute.ModificationValue = modificationValue;
            attribute.RecalculateFinalValue();
            UpdateNewFinalValueOfAttribute(attribute, character.Attributes, character.SkillGroups);
        }

        public void SetCorrosionValue(Attribute attribute, int corrosionValue, Character character)
        {
            attribute.Corrosion = corrosionValue;
            attribute.RecalculateFinalValue();
            UpdateNewFinalValueOfAttribute(attribute, character.Attributes, character.SkillGroups);
        }
        
        public void AddOneExperienceToAttribute(Attribute attribute, List<Attribute> allAttributes, Dictionary<SkillGroupType, SkillGroup> allSkillGroups)
        {
            attribute.Experience += 1;

            while (SkillIncreaseHelper.CanSkillBeIncreased(attribute))
            {
                var requiredExperienceForNextLevel = SkillIncreaseHelper.GetExperienceForNextAttributeLevel(attribute.IncreaseValue);
                attribute.Experience -= requiredExperienceForNextLevel;
                attribute.IncreaseValue++;
                attribute.RecalculateFinalValue();

                UpdateNewFinalValueOfAttribute(attribute, allAttributes, allSkillGroups);
            }
        }

        private IEnumerable<SkillGroupType> AddExperienceToSkillGroup(SkillGroup skillGroup, int experience)
        {
            skillGroup.ExperienceValue += experience;
            while (SkillIncreaseHelper.CanSkillBeIncreased(skillGroup))
            {
                var requiredExperienceForNextLevel = SkillIncreaseHelper.GetExperienceForNextSkillGroupLevel(skillGroup.IncreaseValue);
                skillGroup.ExperienceValue -= requiredExperienceForNextLevel;
                skillGroup.IncreaseValue++;
                skillGroup.RecalculateFinalValue();
                yield return skillGroup.Type;
                UpdateNewBaseValueToSkillsOfGroup(skillGroup);
            }
        }
        public IEnumerable<SkillGroupType> AddExperienceToSkill(Skill skill, SkillGroup skillGroup, int experience)
        {
            skill.ExperienceValue += experience;
            int openSkillGroupExperience = 0;
            while (SkillIncreaseHelper.CanSkillBeIncreased(skill))
            {
                var requiredExperienceForNextLevel = SkillIncreaseHelper.GetExperienceForNextSkillLevel(skill.IncreaseValue);
                skill.ExperienceValue -= requiredExperienceForNextLevel;
                skill.IncreaseValue++;
                skill.RecalculateFinalValue();
                openSkillGroupExperience++;
            }

            if (openSkillGroupExperience > 0)
                return AddExperienceToSkillGroup(skillGroup, openSkillGroupExperience);

            return new List<SkillGroupType>();
        }


        private void UpdateNewBaseValueToSkillsOfGroup(SkillGroup skillgroup)
        {
            foreach (var skill in skillgroup.Skills)
            {
                skill.BaseValue = skillgroup.FinalValue;
                skill.RecalculateFinalValue();
            }
        }
        
        private void UpdateNewFinalValueOfAttribute(Attribute changedAttribute, List<Attribute> allAttributes, Dictionary<SkillGroupType, SkillGroup> allSkillGroups)
        {
            //updating all dependent skillgroups
            var affectedSkillGroupTypes = _ruleRepository.GetSkillGroupsByAttribute(changedAttribute.Type);
            var skillGroups = allSkillGroups
                .Where(pair => affectedSkillGroupTypes
                    .Contains(pair.Key))
                .Select(pair => pair.Value);

            foreach (var affectedSkillGroup in skillGroups)
            {
                var attributeTypesForCalculation = _ruleRepository.GetSkillGroupSources(affectedSkillGroup.Type);

                double tmp = 0;
                foreach (var attributeType in attributeTypesForCalculation)
                {
                    tmp += allAttributes.First(attribute => attribute.Type == attributeType).FinalValue;
                }
                var newBaseValue = (int)Math.Round((tmp / 6), MidpointRounding.AwayFromZero);
                
                affectedSkillGroup.BaseValue = newBaseValue;
                affectedSkillGroup.RecalculateFinalValue();

                UpdateNewBaseValueToSkillsOfGroup(affectedSkillGroup);
            }

            //todo update derived attributes

            //todo update special attribues
        }
    }
}