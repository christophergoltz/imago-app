using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository;
using Imago.Util;
using Attribute = Imago.Models.Attribute;

namespace Imago.ViewModels
{
    public class CharacterViewModel : BindableBase
    {
        public Character Character { get; }
        private readonly IRuleRepository _ruleRepository;
        private bool _editMode;

        public bool EditMode
        {
            get => _editMode;
            set => SetProperty(ref _editMode, value);
        }

        public CharacterViewModel(Character character, IRuleRepository ruleRepository)
        {
            Character = character;
            _ruleRepository = ruleRepository;

            //todo move to character service
            var derivedAttributeTypes = new List<DerivedAttributeType>()
            {
                DerivedAttributeType.Egoregenration,
                DerivedAttributeType.Schadensmod,
                DerivedAttributeType.Traglast,
                DerivedAttributeType.TaktischeBewegung,
                DerivedAttributeType.Sprintreichweite,
                DerivedAttributeType.SprungreichweiteKampf,
                DerivedAttributeType.SprunghoeheKampf,
                DerivedAttributeType.SprungreichweiteAbenteuer,
                DerivedAttributeType.SprunghoeheAbenteuer,
                DerivedAttributeType.SprungreichweiteGesamt,
                DerivedAttributeType.SprunghoeheGesamt,

                //second level attribute for calc
                DerivedAttributeType.BehinderungKampf,
                DerivedAttributeType.BehinderungAbenteuer,
                DerivedAttributeType.BehinderungGesamt
            };
            DerivedAttributes = derivedAttributeTypes.Select(type => new DerivedAttribute(type)).ToList();

            SpecialAttributes = new List<SpecialAttribute>() {new SpecialAttribute(SpecialAttributeType.Initiative)};



        }

        public List<SpecialAttribute> SpecialAttributes { get; set; }
        public List<DerivedAttribute> DerivedAttributes { get; set; }

        public List<DerivedAttribute> CreationDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.Egoregenration ||
                        _.Type == DerivedAttributeType.Schadensmod ||
                        _.Type == DerivedAttributeType.Traglast ||
                        _.Type == DerivedAttributeType.TaktischeBewegung ||
                        _.Type == DerivedAttributeType.Sprintreichweite ||
                        _.Type == DerivedAttributeType.SprungreichweiteGesamt ||
                        _.Type == DerivedAttributeType.SprunghoeheGesamt)
            .ToList();

        public List<DerivedAttribute> FightDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.SprungreichweiteKampf ||
                        _.Type == DerivedAttributeType.SprunghoeheKampf ||
                        _.Type == DerivedAttributeType.Sprintreichweite ||
                        _.Type == DerivedAttributeType.TaktischeBewegung)
            .ToList();

        public List<DerivedAttribute> AdventureDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.SprungreichweiteAbenteuer ||
                        _.Type == DerivedAttributeType.SprunghoeheAbenteuer ||
                        _.Type == DerivedAttributeType.SprungreichweiteGesamt ||
                        _.Type == DerivedAttributeType.SprunghoeheGesamt)
            .ToList();

        public List<DerivedAttribute> HandicapAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.BehinderungKampf ||
                        _.Type == DerivedAttributeType.BehinderungAbenteuer ||
                        _.Type == DerivedAttributeType.BehinderungGesamt)
            .ToList();

        public List<DerivedAttribute> CharacterInfoDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.Egoregenration ||
                                         _.Type == DerivedAttributeType.Schadensmod ||
                                         _.Type == DerivedAttributeType.Traglast)
                .ToList();

        private void UpdateNewBaseValueToSkillsOfGroup(SkillGroupModel skillgroup)
        {
            foreach (var skill in skillgroup.Skills)
            {
                skill.BaseValue = (int) skillgroup.FinalValue;
                skill.RecalculateFinalValue();
            }
        }


        public void SetModificationValue(SkillModel skillModel, int modificationValue)
        {
            skillModel.ModificationValue = modificationValue;
            skillModel.RecalculateFinalValue();
        }

        public void SetModificationValue(SkillGroupModel skillGroupModel, int modificationValue)
        {
            skillGroupModel.ModificationValue = modificationValue;
            skillGroupModel.RecalculateFinalValue();
            UpdateNewBaseValueToSkillsOfGroup(skillGroupModel);
        }

        public void SetModificationValue(Attribute attribute, int modificationValue)
        {
            attribute.ModificationValue = modificationValue;
            attribute.RecalculateFinalValue();
            UpdateNewFinalValueOfAttribute(attribute);
        }

        public void SetModificationValue(SpecialAttribute specialAttribute, int modificationValue)
        {
            specialAttribute.ModificationValue = modificationValue;
            RecalculateSpecialAttributes(specialAttribute);
        }

        public void SetCorrosionValue(Attribute attribute, int corrosionValue)
        {
            attribute.Corrosion = corrosionValue;
            attribute.RecalculateFinalValue();
            UpdateNewFinalValueOfAttribute(attribute);
        }

        public bool CheckTalentRequirement(Dictionary<SkillModelType, int> requirements)
        {
            foreach (var requirement in requirements)
            {
                var skill = Character.SkillGroups.SelectMany(pair => pair.Value.Skills).First(_ => _.Type == requirement.Key);
                if (skill.IncreaseValue < requirement.Value)
                {
                    return false;
                }
            }
            return true;
        }


        public bool CheckMasteryRequirement(Dictionary<SkillGroupModelType, int> requirements)
        {
            foreach (var requirement in requirements)
            {
                var skillGroup = Character.SkillGroups[requirement.Key];
                if (skillGroup.IncreaseValue < requirement.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public void AddOneExperienceToAttributeBySkillGroup(Attribute attribute)
        {
            attribute.ExperienceBySkillGroup += 1;

            //force recalc of increaseablebase
            attribute.TotalExperience = attribute.TotalExperience;

            UpdateNewFinalValueOfAttribute(attribute);
        }


        public void SetExperienceToAttribute(Attribute attribute, int experience)
        {
            attribute.TotalExperience = experience;
            UpdateNewFinalValueOfAttribute(attribute);
        }

        private int SetExperienceToSkillGroup(SkillGroupModel skillGroupModel, int experience)
        {
            var oldIncreaseValue = skillGroupModel.IncreaseValue;
            skillGroupModel.TotalExperience += experience;
            var newIncreaseValue = skillGroupModel.IncreaseValue;
            var openAttributeExperience = newIncreaseValue - oldIncreaseValue;
            return openAttributeExperience;
        }

        public void SetExperienceToSkill(SkillModel skillModel, SkillGroupModel skillGroupModel, int experience)
        {
            var oldIncreaseValue = skillModel.IncreaseValue;
            skillModel.TotalExperience = experience;
            var newIncreaseValue = skillModel.IncreaseValue;
            var openSkillGroupExperience = newIncreaseValue - oldIncreaseValue;
            
            var openAttributeExperience = SetExperienceToSkillGroup(skillGroupModel, openSkillGroupExperience);
            if (openAttributeExperience > 0)
            {
                //add
                for (var i = 0; i < openAttributeExperience; i++)
                {
                    Character.OpenAttributeIncreases.Add(skillGroupModel.Type);
                }
            }
            else if (openAttributeExperience < 0)
            {
                //remove
                for (var i = 0; i < (openAttributeExperience *-1); i++)
                {
                    Character.OpenAttributeIncreases.Remove(skillGroupModel.Type);
                }
            }
        }

        public void RemoveOneExperienceFromSkill(SkillModel skillModel)
        {
            skillModel.TotalExperience -= 1;
            skillModel.RecalculateFinalValue();

            //todo if sw was reduced, take exp from kategoriy
        }
        
        public void UpdateNewFinalValueOfAttribute(Attribute changedAttribute)
        {
            //updating all dependent skillgroups
            var affectedSkillGroupTypes = _ruleRepository.GetSkillGroupsByAttribute(changedAttribute.Type);
            var skillGroups = Character.SkillGroups
                .Where(pair => affectedSkillGroupTypes
                    .Contains(pair.Key))
                .Select(pair => pair.Value);

            foreach (var affectedSkillGroup in skillGroups)
            {
                var attributeTypesForCalculation = _ruleRepository.GetSkillGroupSources(affectedSkillGroup.Type);

                double tmp = 0;
                foreach (var attributeType in attributeTypesForCalculation)
                {
                    tmp += Character.Attributes.First(attribute => attribute.Type == attributeType).FinalValue;
                }

                var newBaseValue = (int) Math.Round((tmp / 6), MidpointRounding.AwayFromZero);

                affectedSkillGroup.BaseValue = newBaseValue;
                affectedSkillGroup.RecalculateFinalValue();

                UpdateNewBaseValueToSkillsOfGroup(affectedSkillGroup);
            }

            //update derived attributes
            foreach (var derivedAttribute in DerivedAttributes)
            {
                switch (derivedAttribute.Type)
                {
                    case DerivedAttributeType.Egoregenration:
                    {
                        var tmp = GetAttributeSum(AttributeType.Willenskraft);
                        var baseValue = tmp / 5;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.Schadensmod:
                    {
                        var tmp = GetAttributeSum(AttributeType.Staerke);
                            var baseValue = (tmp / 10) - 5;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.Traglast:
                    {
                        var tmp = GetAttributeSum(AttributeType.Konstitution, AttributeType.Konstitution,
                            AttributeType.Staerke);
                        var baseValue = tmp / 10;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.TaktischeBewegung:
                    {
                        var tmp = GetAttributeSum(AttributeType.Geschicklichkeit);
                            var baseValue = tmp / 10;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case DerivedAttributeType.Sprintreichweite:
                    {
                        var tmp = GetAttributeSum(AttributeType.Geschicklichkeit);
                            var baseValue = tmp / 5;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                }
            }

            //update special attributes
            RecalculateSpecialAttributes(SpecialAttributes.ToArray());

            //update bodyparts
            foreach (var bodyPart in Character.BodyParts)
            {
                double constFinalValue = GetAttributeSum(AttributeType.Konstitution);

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
            RecalculateHandicapAttributes();
        }

        public void RecalculateSpecialAttributes(params SpecialAttribute[] attributes)
        {
            foreach (var specialAttribute in attributes)
            {
                switch (specialAttribute.Type)
                {
                    case SpecialAttributeType.Initiative:
                    {
                        var tmp = GetAttributeSum(AttributeType.Geschicklichkeit, AttributeType.Geschicklichkeit,
                            AttributeType.Wahrnehmung, AttributeType.Willenskraft);

                        var newValue = tmp / 4;
                        specialAttribute.FinalValue = ((int)Math.Round(newValue, MidpointRounding.AwayFromZero)) + specialAttribute.ModificationValue;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(SpecialAttributeType));
                }
            }
        }

        public void RecalculateHandicapAttributes()
        {
            var loadLimit = GetDerivedAttributeSum(DerivedAttributeType.Traglast);

            //update handicap
            foreach (var handicapAttribute in DerivedAttributes)
            {
                switch (handicapAttribute.Type)
                {
                    case DerivedAttributeType.BehinderungKampf:
                    case DerivedAttributeType.BehinderungAbenteuer:
                    case DerivedAttributeType.BehinderungGesamt:
                    {
                        if (loadLimit == 0)
                        {
                            handicapAttribute.FinalValue = -1;
                            break;
                        }

                        int load = 0;

                        if (handicapAttribute.Type == DerivedAttributeType.BehinderungKampf)
                            load = GetFightLoad();
                        if (handicapAttribute.Type == DerivedAttributeType.BehinderungAbenteuer)
                            load = GetAdventureLoad();
                        if (handicapAttribute.Type == DerivedAttributeType.BehinderungGesamt)
                            load = GetCompleteLoad();

                        handicapAttribute.FinalValue =
                            (int) Math.Round(load / loadLimit, MidpointRounding.AwayFromZero);
                        break;
                    }
                }
            }

            //update derived attributes based on handicap attributes
            foreach (var derivedAttribute in DerivedAttributes)
            {
                switch (derivedAttribute.Type)
                {
                    case DerivedAttributeType.SprungreichweiteKampf:
                    case DerivedAttributeType.SprungreichweiteAbenteuer:
                    case DerivedAttributeType.SprungreichweiteGesamt:
                    {
                        double tmp = GetAttributeSum(AttributeType.Geschicklichkeit, AttributeType.Staerke);
                            
                        if (derivedAttribute.Type == DerivedAttributeType.SprungreichweiteKampf)
                            tmp -= GetFightLoad();
                        if (derivedAttribute.Type == DerivedAttributeType.SprungreichweiteAbenteuer)
                            tmp -= GetAdventureLoad();
                        if (derivedAttribute.Type == DerivedAttributeType.SprungreichweiteGesamt)
                            tmp -= GetCompleteLoad();

                        tmp /= 30;
                        derivedAttribute.FinalValue = Math.Round(tmp, 2);
                        break;
                    }
                    case DerivedAttributeType.SprunghoeheKampf:
                    case DerivedAttributeType.SprunghoeheAbenteuer:
                    case DerivedAttributeType.SprunghoeheGesamt:
                    {
                        double tmp = GetAttributeSum(AttributeType.Geschicklichkeit, AttributeType.Staerke);

                        if (derivedAttribute.Type == DerivedAttributeType.SprunghoeheKampf)
                            tmp -= GetFightLoad();
                        if (derivedAttribute.Type == DerivedAttributeType.SprunghoeheAbenteuer)
                            tmp -= GetAdventureLoad();
                        if (derivedAttribute.Type == DerivedAttributeType.SprunghoeheGesamt)
                            tmp -= GetCompleteLoad();

                        tmp /= 80;
                        derivedAttribute.FinalValue = Math.Round(tmp, 2);
                        break;
                    }
                }
            }
        }

        private int GetFightLoad()
        {
            var fightingArmorLoad = Character.BodyParts.Values.Sum(bodyPart =>
                bodyPart.Armor.Where(armor => armor.Fight).Sum(_ => _.LoadValue));
            var fightingWeaponLoad = Character.Weapons.Sum(weapon => weapon.LoadValue);
            var fightingItemLoad = Character.EquippedItems.Where(item => item.Fight).Sum(_ => _.LoadValue);
            return fightingArmorLoad + fightingWeaponLoad + fightingItemLoad;
        }

        private int GetAdventureLoad()
        {
            var adventureItemLoad = Character.EquippedItems.Where(item => item.Adventure).Sum(_ => _.LoadValue);
            var adventureArmorLoad = Character.BodyParts.Values.Sum(bodyPart =>
                bodyPart.Armor.Where(armor => armor.Adventure).Sum(_ => _.LoadValue));
            var fightingWeaponLoad = Character.Weapons.Sum(weapon => weapon.LoadValue);
            return adventureArmorLoad + fightingWeaponLoad + adventureItemLoad;
        }

        private int GetCompleteLoad()
        {
            var itemLoad = Character.EquippedItems.Sum(_ => _.LoadValue);
            var armorLoad = Character.BodyParts.Values.Sum(bodyPart => bodyPart.Armor.Sum(_ => _.LoadValue));
            var fightLoad = Character.Weapons.Sum(weapon => weapon.LoadValue);
            return armorLoad + fightLoad + itemLoad;
        }

        private double GetAttributeSum(params AttributeType[] attributes)
        {
            return attributes.Sum(attributeType => Character.Attributes.First(_ => _.Type == attributeType).FinalValue);
        }
        private double GetDerivedAttributeSum(params DerivedAttributeType[] attributes)
        {
            return attributes.Sum(attributeType => DerivedAttributes.First(_ => _.Type == attributeType).FinalValue);
        }
    }
}