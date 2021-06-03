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
        IEnumerable<SkillGroupType> AddOneExperienceToSkill(Skill skill, SkillGroup skillGroup);
        void RemoveOneExperienceFromSkill(Skill skill);
        void SetModificationValue(Skill skill, int modificationValue);
        void SetModificationValue(SkillGroup skillGroup, int modificationValue);
        void SetModificationValue(Attribute attribute, int modificationValue, Character character);
        void SetModificationValue(SpecialAttribute specialAttribute, int modificationValue, Character character);
        void AddOneExperienceToAttribute(Attribute attribute, Character character);
        void RecalculateHandicapAttributes(Character character);
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
            UpdateNewFinalValueOfAttribute(attribute, character);
        }

        public void SetModificationValue(SpecialAttribute specialAttribute, int modificationValue, Character character)
        {
            specialAttribute.ModificationValue = modificationValue;
            specialAttribute.RecalculateFinalValue();
        }

        public void SetCorrosionValue(Attribute attribute, int corrosionValue, Character character)
        {
            attribute.Corrosion = corrosionValue;
            attribute.RecalculateFinalValue();
            UpdateNewFinalValueOfAttribute(attribute, character);
        }

        public void AddOneExperienceToAttribute(Attribute attribute, Character character)
        {
            attribute.Experience += 1;

            while (SkillIncreaseHelper.CanSkillBeIncreased(attribute))
            {
                var requiredExperienceForNextLevel =
                    SkillIncreaseHelper.GetExperienceForNextAttributeLevel(attribute.IncreaseValue);
                attribute.Experience -= requiredExperienceForNextLevel;
                attribute.IncreaseValue++;
                attribute.RecalculateFinalValue();

                UpdateNewFinalValueOfAttribute(attribute, character);
            }
        }

        private IEnumerable<SkillGroupType> AddExperienceToSkillGroup(SkillGroup skillGroup, int experience)
        {
            skillGroup.ExperienceValue += experience;
            while (SkillIncreaseHelper.CanSkillBeIncreased(skillGroup))
            {
                var requiredExperienceForNextLevel =
                    SkillIncreaseHelper.GetExperienceForNextSkillGroupLevel(skillGroup.IncreaseValue);
                skillGroup.ExperienceValue -= requiredExperienceForNextLevel;
                skillGroup.IncreaseValue++;
                skillGroup.RecalculateFinalValue();
                yield return skillGroup.Type;
                UpdateNewBaseValueToSkillsOfGroup(skillGroup);
            }
        }

        public IEnumerable<SkillGroupType> AddOneExperienceToSkill(Skill skill, SkillGroup skillGroup)
        {
            skill.ExperienceValue += 1;
            int openSkillGroupExperience = 0;
            while (SkillIncreaseHelper.CanSkillBeIncreased(skill))
            {
                var requiredExperienceForNextLevel =
                    SkillIncreaseHelper.GetExperienceForNextSkillLevel(skill.IncreaseValue);
                skill.ExperienceValue -= requiredExperienceForNextLevel;
                skill.IncreaseValue++;
                skill.RecalculateFinalValue();
                openSkillGroupExperience++;
            }

            if (openSkillGroupExperience > 0)
                return AddExperienceToSkillGroup(skillGroup, openSkillGroupExperience);

            return new List<SkillGroupType>();
        }

        public void RemoveOneExperienceFromSkill(Skill skill)
        {
            if (skill.ExperienceValue == 0)
                throw new InvalidOperationException("Cant remove experience from skill, value is alredy 0");

            skill.ExperienceValue -= 1;
        }


        private void UpdateNewBaseValueToSkillsOfGroup(SkillGroup skillgroup)
        {
            foreach (var skill in skillgroup.Skills)
            {
                skill.BaseValue = skillgroup.FinalValue;
                skill.RecalculateFinalValue();
            }
        }

        private void UpdateNewFinalValueOfAttribute(Attribute changedAttribute, Character character)
        {
            //updating all dependent skillgroups
            var affectedSkillGroupTypes = _ruleRepository.GetSkillGroupsByAttribute(changedAttribute.Type);
            var skillGroups = character.SkillGroups
                .Where(pair => affectedSkillGroupTypes
                    .Contains(pair.Key))
                .Select(pair => pair.Value);

            foreach (var affectedSkillGroup in skillGroups)
            {
                var attributeTypesForCalculation = _ruleRepository.GetSkillGroupSources(affectedSkillGroup.Type);

                double tmp = 0;
                foreach (var attributeType in attributeTypesForCalculation)
                {
                    tmp += character.Attributes.First(attribute => attribute.Type == attributeType).FinalValue;
                }

                var newBaseValue = (int) Math.Round((tmp / 6), MidpointRounding.AwayFromZero);

                affectedSkillGroup.BaseValue = newBaseValue;
                affectedSkillGroup.RecalculateFinalValue();

                UpdateNewBaseValueToSkillsOfGroup(affectedSkillGroup);
            }

            //update derived attributes
            foreach (var derivedAttribute in character.DerivedAttributes)
            {
                switch (derivedAttribute.Type)
                {
                    case DerivedAttributeType.Egoregenration:
                    {
                        var tmp = character.Attributes.GetFinalValueOfAttributeType(AttributeType.Willenskraft);
                        var baseValue = tmp / 5;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.Schadensmod:
                    {
                        var tmp = character.Attributes.GetFinalValueOfAttributeType(AttributeType.Staerke);
                        var baseValue = (tmp / 10) - 5;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.Traglast:
                    {
                        var tmp = character.Attributes.GetFinalValueOfAttributeType(AttributeType.Konstitution)
                                  + character.Attributes.GetFinalValueOfAttributeType(AttributeType.Konstitution)
                                  + character.Attributes.GetFinalValueOfAttributeType(AttributeType.Staerke);
                        var baseValue = tmp / 10;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.Sprungreichweite:
                    case DerivedAttributeType.Sprunghoehe:
                    case DerivedAttributeType.TaktischeBewegung:
                    case DerivedAttributeType.Sprintreichweite:
                    {
                        derivedAttribute.FinalValue = -1;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //update special attributes
            foreach (var specialAttribute in character.SpecialAttributes)
            {
                switch (specialAttribute.Type)
                {
                    case SpecialAttributeType.Initiative:
                    {
                        var tmp =
                            (character.Attributes.GetFinalValueOfAttributeType(AttributeType.Geschicklichkeit) * 2) +
                            character.Attributes.GetFinalValueOfAttributeType(AttributeType.Wahrnehmung) +
                            character.Attributes.GetFinalValueOfAttributeType(AttributeType.Willenskraft);

                        var newValue = tmp / 4;
                        specialAttribute.BaseValue = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        specialAttribute.RecalculateFinalValue();
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(SpecialAttributeType));
                }
            }

            //update bodyparts
            foreach (var bodyPart in character.BodyParts)
            {
                var constFinalValue = character.Attributes.GetFinalValueOfAttributeType(AttributeType.Konstitution);

                switch (bodyPart.Key)
                {
                    case BodyPartType.Kopf:
                    {
                        var newValue = (constFinalValue / 15) + 3;
                        bodyPart.Value.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case BodyPartType.Torso:
                    {
                        var newValue = (constFinalValue / 6) + 2;
                        bodyPart.Value.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case BodyPartType.ArmLinks:
                    case BodyPartType.ArmRechts:
                    {
                        var newValue = (constFinalValue / 10) + 1;
                        bodyPart.Value.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case BodyPartType.BeinLinks:
                    case BodyPartType.BeinRechts:
                    {
                        var newValue = (constFinalValue / 7) + 2;
                        bodyPart.Value.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //update handicap
            RecalculateHandicapAttributes(character);
        }

        //todo recalc when item has changed
        public void RecalculateHandicapAttributes(Character character)
        {
            var loadLimit = character.DerivedAttributes.GetFinalValueOfAttributeType(DerivedAttributeType.Traglast);

            //fighting load
            var armor = character.BodyParts.Select(pair => pair.Value).SelectMany(part => part.Armor)
                .Sum(_ => _.LoadValue);
            var weaponLoad = character.Weapon1?.LoadValue ??
                             0 + character.Weapon2?.LoadValue ?? 0 + character.Weapon3?.LoadValue ?? 0;
            var otherFightingGear = character.EquippedItems.Where(item => item.Fight).Sum(_ => _.LoadValue);

            //adventure load
            var adventureGear = character.EquippedItems.Where(item => !item.Fight && item.Adventure)
                .Sum(_ => _.LoadValue);

            //other load
            var otherGear = character.EquippedItems.Where(item => !item.Fight && !item.Adventure).Sum(_ => _.LoadValue);

            //update handicap
            foreach (var handicapAttribute in character.Handicap)
            {
                switch (handicapAttribute.Type)
                {
                    case DerivedAttributeType.BehinderungKampf:
                    {
                        if (loadLimit == 0)
                        {
                            handicapAttribute.FinalValue = -1;
                            break;
                        }

                        var load = (armor + weaponLoad + otherFightingGear) / loadLimit;
                        handicapAttribute.FinalValue = (int) Math.Round(load, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.BehinderungAbenteuer:
                    {
                        if (loadLimit == 0)
                        {
                            handicapAttribute.FinalValue = -1;
                            break;
                        }

                        var load = (armor + weaponLoad + otherFightingGear + adventureGear) / loadLimit;
                        handicapAttribute.FinalValue = (int) Math.Round(load, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.BehinderungGesamt:
                    {
                        if (loadLimit == 0)
                        {
                            handicapAttribute.FinalValue = -1;
                            break;
                        }

                        var load = (armor + weaponLoad + otherFightingGear + adventureGear + otherGear) / loadLimit;
                        handicapAttribute.FinalValue = (int) Math.Round(load, MidpointRounding.AwayFromZero);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}