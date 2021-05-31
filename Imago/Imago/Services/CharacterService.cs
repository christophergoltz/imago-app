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
        void AddCorrosion(Attribute attribute, int corrosion);
        IEnumerable<SkillGroupType> AddExperienceToSkill(Skill skill, SkillGroup skillGroup, int experience);
        void SetModificationValue(Skill skill, int newModificationValue);
        void SetModificationValue(SkillGroup skillGroup, int newModificationValue);
        void SetModificationValue(Attribute attribute, int newModificationValue, Character character);
    }

    public class CharacterService : ICharacterService
    {
        private readonly IRuleRepository _ruleRepository;

        public CharacterService(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public void SetModificationValue(Skill skill, int newModificationValue)
        {
            skill.ModificationValue = newModificationValue;
            skill.RecalculateFinalValue();
        }

        public void SetModificationValue(SkillGroup skillGroup, int newModificationValue)
        {
            skillGroup.ModificationValue = newModificationValue;
            skillGroup.RecalculateFinalValue();

            foreach (var skill in skillGroup.Skills)
            {
                skill.NaturalValue = skillGroup.FinalValue;
                skill.RecalculateFinalValue();
            }
        }

        public void SetModificationValue(Attribute attribute, int newModificationValue, Character character)
        {
            attribute.ModificationValue = newModificationValue;
            attribute.RecalculateFinalValue();

            var affectedSkillGroupTypes = _ruleRepository.GetSkillGroupsByAttribute(attribute.Type);
            var skillGroups = character.SkillGroups
                .Where(pair => affectedSkillGroupTypes
                    .Contains(pair.Key))
                .Select(pair => pair.Value);
            
            foreach (var skillGroup in skillGroups)
            {
                skillGroup.NaturalValue = attribute.FinalValue;
                skillGroup.RecalculateFinalValue();

                foreach (var skill in skillGroup.Skills)
                {
                    skill.NaturalValue = skillGroup.FinalValue;
                    skill.RecalculateFinalValue();
                }
            }

            //todo recalculate professions
        }

        public void AddCorrosion(Attribute attribute, int corrosion)
        {
            attribute.Corrosion += corrosion;
            attribute.RecalculateFinalValue();
            UpdateDependentSkills(attribute.Type, attribute.FinalValue);
        }

        //todo alle nw von skillgroup andern und alle skills neu berechnen
        private void UpdateDependentSkills(AttributeType type, int newFinalValue)
        {
            //todo
        }

      
        public IEnumerable<SkillGroupType> AddExperienceToSkill(Skill skill, SkillGroup skillGroup, int experience)
        {
            skill.Experience += experience;
            int openSkillGroupExperience = 0;
            while (SkillIncreaseHelper.CanSkillBeIncreased(skill))
            {
                var requiredExperienceForNextLevel = SkillIncreaseHelper.GetExperienceForNextLevel(skill);
                skill.Experience -= requiredExperienceForNextLevel;
                skill.IncreaseValue++;
                skill.RecalculateFinalValue();
                openSkillGroupExperience++;
            }

            if (openSkillGroupExperience > 0)
                return AddExperienceToSkillGroup(skillGroup, openSkillGroupExperience);

            return new List<SkillGroupType>();
        }

        private IEnumerable<SkillGroupType> AddExperienceToSkillGroup(SkillGroup skillGroup, int experience)
        {
            skillGroup.Experience += experience;
            while (SkillIncreaseHelper.CanSkillBeIncreased(skillGroup))
            {
                var requiredExperienceForNextLevel = SkillIncreaseHelper.GetExperienceForNextLevel(skillGroup);
                skillGroup.Experience -= requiredExperienceForNextLevel;
                skillGroup.IncreaseValue++;
                skillGroup.RecalculateFinalValue();
                yield return skillGroup.Type;
                UpdateNewNaturalValuesOfGroup(skillGroup, skillGroup.FinalValue);
            }
        }

        private void UpdateNewNaturalValuesOfGroup(SkillGroup skillgroup, int newNaturalValue)
        {
            foreach (var skill in skillgroup.Skills)
            {
                skill.NaturalValue = newNaturalValue;
                skill.RecalculateFinalValue();
            }
        }
    }
}