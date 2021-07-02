using System;
using System.Collections.Generic;
using System.Linq;

namespace ImagoApp.ViewModels
{
    public class CharacterViewModel : Util.BindableBase
    {
        public Models.Character Character { get; }
        private readonly Repository.IRuleRepository _ruleRepository;
        private bool _editMode;

        public bool EditMode
        {
            get => _editMode;
            set => SetProperty(ref _editMode, value);
        }

        public CharacterViewModel(Models.Character character, Repository.IRuleRepository ruleRepository)
        {
            Character = character;
            _ruleRepository = ruleRepository;

            //todo move to character service
            var derivedAttributeTypes = new List<Models.Enum.DerivedAttributeType>()
            {
                Models.Enum.DerivedAttributeType.Egoregenration,
                Models.Enum.DerivedAttributeType.Schadensmod,
                Models.Enum.DerivedAttributeType.Traglast,
                Models.Enum.DerivedAttributeType.TaktischeBewegung,
                Models.Enum.DerivedAttributeType.Sprintreichweite,
                Models.Enum.DerivedAttributeType.SprungreichweiteKampf,
                Models.Enum.DerivedAttributeType.SprunghoeheKampf,
                Models.Enum.DerivedAttributeType.SprungreichweiteAbenteuer,
                Models.Enum.DerivedAttributeType.SprunghoeheAbenteuer,
                Models.Enum.DerivedAttributeType.SprungreichweiteGesamt,
                Models.Enum.DerivedAttributeType.SprunghoeheGesamt,

                //second level attribute for calc
                Models.Enum.DerivedAttributeType.BehinderungKampf,
                Models.Enum.DerivedAttributeType.BehinderungAbenteuer,
                Models.Enum.DerivedAttributeType.BehinderungGesamt
            };
            DerivedAttributes = derivedAttributeTypes.Select(type => new Models.DerivedAttribute(type)).ToList();

            SpecialAttributes = new List<Models.SpecialAttribute>() {new Models.SpecialAttribute(Models.Enum.SpecialAttributeType.Initiative)};



        }

        public List<Models.SpecialAttribute> SpecialAttributes { get; set; }
        public List<Models.DerivedAttribute> DerivedAttributes { get; set; }

        public List<Models.DerivedAttribute> CreationDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == Models.Enum.DerivedAttributeType.Egoregenration ||
                        _.Type == Models.Enum.DerivedAttributeType.Schadensmod ||
                        _.Type == Models.Enum.DerivedAttributeType.Traglast ||
                        _.Type == Models.Enum.DerivedAttributeType.TaktischeBewegung ||
                        _.Type == Models.Enum.DerivedAttributeType.Sprintreichweite ||
                        _.Type == Models.Enum.DerivedAttributeType.SprungreichweiteGesamt ||
                        _.Type == Models.Enum.DerivedAttributeType.SprunghoeheGesamt)
            .ToList();

        public List<Models.DerivedAttribute> FightDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == Models.Enum.DerivedAttributeType.SprungreichweiteKampf ||
                        _.Type == Models.Enum.DerivedAttributeType.SprunghoeheKampf ||
                        _.Type == Models.Enum.DerivedAttributeType.Sprintreichweite ||
                        _.Type == Models.Enum.DerivedAttributeType.TaktischeBewegung)
            .ToList();

        public List<Models.DerivedAttribute> AdventureDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == Models.Enum.DerivedAttributeType.SprungreichweiteAbenteuer ||
                        _.Type == Models.Enum.DerivedAttributeType.SprunghoeheAbenteuer ||
                        _.Type == Models.Enum.DerivedAttributeType.SprungreichweiteGesamt ||
                        _.Type == Models.Enum.DerivedAttributeType.SprunghoeheGesamt)
            .ToList();

        public List<Models.DerivedAttribute> HandicapAttributes => DerivedAttributes
            .Where(_ => _.Type == Models.Enum.DerivedAttributeType.BehinderungKampf ||
                        _.Type == Models.Enum.DerivedAttributeType.BehinderungAbenteuer ||
                        _.Type == Models.Enum.DerivedAttributeType.BehinderungGesamt)
            .ToList();

        public List<Models.DerivedAttribute> CharacterInfoDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == Models.Enum.DerivedAttributeType.Egoregenration ||
                                         _.Type == Models.Enum.DerivedAttributeType.Schadensmod ||
                                         _.Type == Models.Enum.DerivedAttributeType.Traglast)
                .ToList();

        private void UpdateNewBaseValueToSkillsOfGroup(Models.SkillGroupModel skillgroup)
        {
            foreach (var skill in skillgroup.Skills)
            {
                skill.BaseValue = (int) skillgroup.FinalValue;
                Util.SkillHelper.RecalculateFinalValue(skill);
            }
        }


        public void SetModificationValue(Models.SkillModel skillModel, int modificationValue)
        {
            skillModel.ModificationValue = modificationValue;
            Util.SkillHelper.RecalculateFinalValue(skillModel);
        }

        public void SetModificationValue(Models.SkillGroupModel skillGroupModel, int modificationValue)
        {
            skillGroupModel.ModificationValue = modificationValue;
            Util.SkillHelper.RecalculateFinalValue(skillGroupModel);
            UpdateNewBaseValueToSkillsOfGroup(skillGroupModel);
        }

        public void SetModificationValue(Models.Attribute attribute, int modificationValue)
        {
            attribute.ModificationValue = modificationValue;
            Util.SkillHelper.RecalculateFinalValue(attribute);
            UpdateNewFinalValueOfAttribute(attribute);
        }

        public void SetModificationValue(Models.SpecialAttribute specialAttribute, int modificationValue)
        {
            specialAttribute.ModificationValue = modificationValue;
            RecalculateSpecialAttributes(specialAttribute);
        }

        public void SetCorrosionValue(Models.Attribute attribute, int corrosionValue)
        {
            attribute.Corrosion = corrosionValue;
            Util.SkillHelper.RecalculateFinalValue(attribute);
            UpdateNewFinalValueOfAttribute(attribute);
        }

        public bool CheckTalentRequirement(Dictionary<Models.SkillModelType, int> requirements)
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


        public bool CheckMasteryRequirement(Dictionary<Models.Enum.SkillGroupModelType, int> requirements)
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

        public void AddOneExperienceToAttributeBySkillGroup(Models.Attribute attribute)
        {
            attribute.ExperienceBySkillGroup += 1;

            //force recalc of increaseablebase
            attribute.TotalExperience = attribute.TotalExperience;

            UpdateNewFinalValueOfAttribute(attribute);
        }


        public void SetExperienceToAttribute(Models.Attribute attribute, int experience)
        {
            attribute.TotalExperience = experience;
            UpdateNewFinalValueOfAttribute(attribute);
        }

        private int SetExperienceToSkillGroup(Models.SkillGroupModel skillGroupModel, int experience)
        {
            var oldIncreaseValue = skillGroupModel.IncreaseValue;
            skillGroupModel.TotalExperience += experience;
            var newIncreaseValue = skillGroupModel.IncreaseValue;
            var openAttributeExperience = newIncreaseValue - oldIncreaseValue;
            return openAttributeExperience;
        }

        public void SetExperienceToSkill(Models.SkillModel skillModel, Models.SkillGroupModel skillGroupModel, int experience)
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
                //todo this should never happen
                throw new InvalidOperationException("Cannot reduce attribute");
                //remove
                for (var i = 0; i < (openAttributeExperience *-1); i++)
                {
#warning  todo Character.AttributeIncreases.Remove(skillGroupModel.Type);
                }
            }
        }

        public void RemoveOneExperienceFromSkill(Models.SkillModel skillModel)
        {
            skillModel.TotalExperience -= 1;
            Util.SkillHelper.RecalculateFinalValue(skillModel);

            //todo if sw was reduced, take exp from kategoriy
        }
        
        public void UpdateNewFinalValueOfAttribute(Models.Attribute changedAttribute)
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
                Util.SkillHelper.RecalculateFinalValue(affectedSkillGroup);

                UpdateNewBaseValueToSkillsOfGroup(affectedSkillGroup);
            }

            //update derived attributes
            foreach (var derivedAttribute in DerivedAttributes)
            {
                switch (derivedAttribute.Type)
                {
                    case Models.Enum.DerivedAttributeType.Egoregenration:
                    {
                        var tmp = GetAttributeSum(Models.Enum.AttributeType.Willenskraft);
                        var baseValue = tmp / 5;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case Models.Enum.DerivedAttributeType.Schadensmod:
                    {
                        var tmp = GetAttributeSum(Models.Enum.AttributeType.Staerke);
                            var baseValue = (tmp / 10) - 5;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case Models.Enum.DerivedAttributeType.Traglast:
                    {
                        var tmp = GetAttributeSum(Models.Enum.AttributeType.Konstitution, Models.Enum.AttributeType.Konstitution,
                            Models.Enum.AttributeType.Staerke);
                        var baseValue = tmp / 10;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case Models.Enum.DerivedAttributeType.TaktischeBewegung:
                    {
                        var tmp = GetAttributeSum(Models.Enum.AttributeType.Geschicklichkeit);
                            var baseValue = tmp / 10;
                        derivedAttribute.FinalValue = (int) Math.Round(baseValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case Models.Enum.DerivedAttributeType.Sprintreichweite:
                    {
                        var tmp = GetAttributeSum(Models.Enum.AttributeType.Geschicklichkeit);
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
                double constFinalValue = GetAttributeSum(Models.Enum.AttributeType.Konstitution);

                switch (bodyPart.Key)
                {
                    case Models.Enum.BodyPartType.Kopf:
                    {
                        var newValue = (constFinalValue / 15) + 3;
                        bodyPart.Value.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case Models.Enum.BodyPartType.Torso:
                    {
                        var newValue = (constFinalValue / 6) + 2;
                        bodyPart.Value.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case Models.Enum.BodyPartType.ArmLinks:
                    case Models.Enum.BodyPartType.ArmRechts:
                    {
                        var newValue = (constFinalValue / 10) + 1;
                        bodyPart.Value.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case Models.Enum.BodyPartType.BeinLinks:
                    case Models.Enum.BodyPartType.BeinRechts:
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

        public void RecalculateSpecialAttributes(params Models.SpecialAttribute[] attributes)
        {
            foreach (var specialAttribute in attributes)
            {
                switch (specialAttribute.Type)
                {
                    case Models.Enum.SpecialAttributeType.Initiative:
                    {
                        var tmp = GetAttributeSum(Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Geschicklichkeit,
                            Models.Enum.AttributeType.Wahrnehmung, Models.Enum.AttributeType.Willenskraft);

                        var newValue = tmp / 4;
                        specialAttribute.FinalValue = ((int)Math.Round(newValue, MidpointRounding.AwayFromZero)) + specialAttribute.ModificationValue;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Models.Enum.SpecialAttributeType));
                }
            }
        }

        public void RecalculateHandicapAttributes()
        {
            var loadLimit = GetDerivedAttributeSum(Models.Enum.DerivedAttributeType.Traglast);

            //update handicap
            foreach (var handicapAttribute in DerivedAttributes)
            {
                switch (handicapAttribute.Type)
                {
                    case Models.Enum.DerivedAttributeType.BehinderungKampf:
                    case Models.Enum.DerivedAttributeType.BehinderungAbenteuer:
                    case Models.Enum.DerivedAttributeType.BehinderungGesamt:
                    {
                        if (loadLimit == 0)
                        {
                            handicapAttribute.FinalValue = -1;
                            break;
                        }

                        int load = 0;

                        if (handicapAttribute.Type == Models.Enum.DerivedAttributeType.BehinderungKampf)
                            load = GetFightLoad();
                        if (handicapAttribute.Type == Models.Enum.DerivedAttributeType.BehinderungAbenteuer)
                            load = GetAdventureLoad();
                        if (handicapAttribute.Type == Models.Enum.DerivedAttributeType.BehinderungGesamt)
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
                    case Models.Enum.DerivedAttributeType.SprungreichweiteKampf:
                    case Models.Enum.DerivedAttributeType.SprungreichweiteAbenteuer:
                    case Models.Enum.DerivedAttributeType.SprungreichweiteGesamt:
                    {
                        double tmp = GetAttributeSum(Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Staerke);
                            
                        if (derivedAttribute.Type == Models.Enum.DerivedAttributeType.SprungreichweiteKampf)
                            tmp -= GetFightLoad();
                        if (derivedAttribute.Type == Models.Enum.DerivedAttributeType.SprungreichweiteAbenteuer)
                            tmp -= GetAdventureLoad();
                        if (derivedAttribute.Type == Models.Enum.DerivedAttributeType.SprungreichweiteGesamt)
                            tmp -= GetCompleteLoad();

                        tmp /= 30;
                        derivedAttribute.FinalValue = Math.Round(tmp, 2);
                        break;
                    }
                    case Models.Enum.DerivedAttributeType.SprunghoeheKampf:
                    case Models.Enum.DerivedAttributeType.SprunghoeheAbenteuer:
                    case Models.Enum.DerivedAttributeType.SprunghoeheGesamt:
                    {
                        double tmp = GetAttributeSum(Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Staerke);

                        if (derivedAttribute.Type == Models.Enum.DerivedAttributeType.SprunghoeheKampf)
                            tmp -= GetFightLoad();
                        if (derivedAttribute.Type == Models.Enum.DerivedAttributeType.SprunghoeheAbenteuer)
                            tmp -= GetAdventureLoad();
                        if (derivedAttribute.Type == Models.Enum.DerivedAttributeType.SprunghoeheGesamt)
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

        private double GetAttributeSum(params Models.Enum.AttributeType[] attributes)
        {
            return attributes.Sum(attributeType => Character.Attributes.First(_ => _.Type == attributeType).FinalValue);
        }
        private double GetDerivedAttributeSum(params Models.Enum.DerivedAttributeType[] attributes)
        {
            return attributes.Sum(attributeType => DerivedAttributes.First(_ => _.Type == attributeType).FinalValue);
        }
    }
}