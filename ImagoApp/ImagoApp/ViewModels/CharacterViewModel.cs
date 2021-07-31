using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using ImagoApp.Util;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;

namespace ImagoApp.ViewModels
{
    public class CharacterViewModel : BindableBase
    {
        public CharacterModel CharacterModel { get; }
        private bool _editMode;

        public bool EditMode
        {
            get => _editMode;
            set => SetProperty(ref _editMode, value);
        }

        public AttributeModel CharismaAttribute => CharacterModel.Attributes.First(model => model.Type == AttributeType.Charisma);
        public AttributeModel GeschicklichkeitAttribute => CharacterModel.Attributes.First(model => model.Type == AttributeType.Geschicklichkeit);
        public AttributeModel IntelligenzAttribute => CharacterModel.Attributes.First(model => model.Type == AttributeType.Intelligenz);
        public AttributeModel KonstitutionAttribute => CharacterModel.Attributes.First(model => model.Type == AttributeType.Konstitution);
        public AttributeModel StaerkeAttribute => CharacterModel.Attributes.First(model => model.Type == AttributeType.Staerke);
        public AttributeModel WahrnehmungAttribute => CharacterModel.Attributes.First(model => model.Type == AttributeType.Wahrnehmung);
        public AttributeModel WillenskraftAttribute => CharacterModel.Attributes.First(model => model.Type == AttributeType.Willenskraft);
        
        public CharacterViewModel(CharacterModel characterModel)
        {
            CharacterModel = characterModel;

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
            DerivedAttributes = derivedAttributeTypes.Select(type => new DerivedAttributeModel(type)).ToList();

            SpecialAttributes = new List<SpecialAttributeModel>() {new SpecialAttributeModel(SpecialAttributeType.Initiative)};



        }

        public List<SpecialAttributeModel> SpecialAttributes { get; set; }
        public List<DerivedAttributeModel> DerivedAttributes { get; set; }

        public List<DerivedAttributeModel> FightDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.SprungreichweiteKampf ||
                        _.Type == DerivedAttributeType.SprunghoeheKampf ||
                        _.Type == DerivedAttributeType.Sprintreichweite ||
                        _.Type == DerivedAttributeType.TaktischeBewegung)
            .ToList();

        public List<DerivedAttributeModel> AdventureDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.SprungreichweiteKampf ||
                        _.Type == DerivedAttributeType.SprunghoeheKampf ||
                        _.Type == DerivedAttributeType.SprungreichweiteAbenteuer ||
                        _.Type == DerivedAttributeType.SprunghoeheAbenteuer ||
                        _.Type == DerivedAttributeType.SprungreichweiteGesamt ||
                        _.Type == DerivedAttributeType.SprunghoeheGesamt)
            .ToList();

        public List<DerivedAttributeModel> HandicapAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.BehinderungKampf ||
                        _.Type == DerivedAttributeType.BehinderungAbenteuer ||
                        _.Type == DerivedAttributeType.BehinderungGesamt)
            .ToList();

        public List<DerivedAttributeModel> CharacterInfoDerivedAttributes => DerivedAttributes
            .Where(_ => _.Type == DerivedAttributeType.Egoregenration ||
                                         _.Type == DerivedAttributeType.Schadensmod ||
                                         _.Type == DerivedAttributeType.Traglast)
                .ToList();

        private void UpdateNewBaseValueToSkillsOfGroup(SkillGroupModel skillgroup)
        {
            foreach (var skill in skillgroup.Skills)
            {
                skill.BaseValue = (int) skillgroup.FinalValue;
                SkillExtensions.RecalculateFinalValue(skill);
            }
        }


        public void SetModificationValue(SkillModel skillModel, int modificationValue)
        {
            skillModel.ModificationValue = modificationValue;
            SkillExtensions.RecalculateFinalValue(skillModel);
        }

        public void SetModificationValue(SkillGroupModel skillGroupModel, int modificationValue)
        {
            skillGroupModel.ModificationValue = modificationValue;
            SkillExtensions.RecalculateFinalValue(skillGroupModel);
            UpdateNewBaseValueToSkillsOfGroup(skillGroupModel);
        }

        public void SetNaturalValue(AttributeModel attributeModel, int naturalValue)
        {
            attributeModel.NaturalValue = naturalValue;
            SkillExtensions.RecalculateFinalValue(attributeModel);
            UpdateNewFinalValueOfAttribute(attributeModel);
        }

        public void SetModificationValue(AttributeModel attributeModel, int modificationValue)
        {
            attributeModel.ModificationValue = modificationValue;
            SkillExtensions.RecalculateFinalValue(attributeModel);
            UpdateNewFinalValueOfAttribute(attributeModel);
        }

        public void SetModificationValue(SpecialAttributeModel specialAttributeModel, int modificationValue)
        {
            specialAttributeModel.ModificationValue = modificationValue;
            RecalculateSpecialAttributes(specialAttributeModel);
        }

        public void SetCorrosionValue(AttributeModel attributeModel, int corrosionValue)
        {
            attributeModel.Corrosion = corrosionValue;
            SkillExtensions.RecalculateFinalValue(attributeModel);
            UpdateNewFinalValueOfAttribute(attributeModel);
        }

        public bool CheckTalentRequirement(List<SkillRequirementModel> requirements)
        {
            foreach (var requirement in requirements)
            {
                var skill = CharacterModel.SkillGroups.SelectMany(pair => pair.Skills).First(_ => _.Type == requirement.Type);
                if (skill.IncreaseValue < requirement.Value)
                {
                    return false;
                }
            }
            return true;
        }


        public bool CheckMasteryRequirement(List<SkillGroupRequirementModel> requirements)
        {
            foreach (var requirement in requirements)
            {
                var skillGroup = CharacterModel.SkillGroups.First(_=> _.Type == requirement.Type);
                if (skillGroup.IncreaseValue < requirement.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public void AddOneExperienceToAttributeBySkillGroup(AttributeModel attributeModel)
        {
            attributeModel.ExperienceBySkillGroup += 1;

            //force recalc of increaseablebase
            attributeModel.TotalExperience = attributeModel.TotalExperience;

            UpdateNewFinalValueOfAttribute(attributeModel);
        }

        public void SetSpecialExperienceToAttribute(AttributeModel attributeModel, int specialExperience)
        {
            attributeModel.SpecialExperience = specialExperience;

            //force recalc
            attributeModel.TotalExperience = attributeModel.TotalExperience;

            UpdateNewFinalValueOfAttribute(attributeModel);
        }


        public void SetExperienceToAttribute(AttributeModel attributeModel, int experience)
        {
            attributeModel.TotalExperience = experience;
            UpdateNewFinalValueOfAttribute(attributeModel);
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
                    CharacterModel.OpenAttributeIncreases.Add(skillGroupModel.Type);
                }
            }
            else if (openAttributeExperience < 0)
            {
                var leftOverExperienceToReduce = 0;

                //remove
                for (var i = 0; i < (openAttributeExperience * -1); i++)
                {
                    if (CharacterModel.OpenAttributeIncreases.Contains(skillGroupModel.Type))
                        CharacterModel.OpenAttributeIncreases.Remove(skillGroupModel.Type);
                    else
                    {
                        leftOverExperienceToReduce++;
                    }
                }

                if (leftOverExperienceToReduce > 0)
                {
                    //Cannot reduce spent attribute experience
                    UserDialogs.Instance.Alert(
                        $"Durch die SW-Reduzierung der Fertigkeitskategorie {skillGroupModel.Type} müssen die Erfahrungpunkt(e) eines Attributes um {leftOverExperienceToReduce} reduziert werden, " +
                        $"welche schon Ausgegeben wurde.{Environment.NewLine}{Environment.NewLine}Momentan wird dies nicht unterstüzt und die Erfahrungspunkte der Attribute weichen nun möglicherweise vom Regelwerk ab." +
                        "Ggf. müssen die Attributs-Erfahrungspunkte neu verteilt werden",
                        "Attributs-Erfahrung reduzieren");
                    Crashes.TrackError(new Exception("Unable to reduce attributeexperience"));
                }
            }
        }

        public void RemoveOneExperienceFromSkill(SkillModel skillModel)
        {
            skillModel.TotalExperience -= 1;
            SkillExtensions.RecalculateFinalValue(skillModel);

            //todo if sw was reduced, take exp from kategoriy
        }
        
        public void UpdateNewFinalValueOfAttribute(AttributeModel changedAttributeModel)
        {
            //updating all dependent skillgroups
            var affectedSkillGroupTypes = RuleConstants.GetSkillGroupsByAttribute(changedAttributeModel.Type);
            var skillGroups = CharacterModel.SkillGroups
                .Where(model => affectedSkillGroupTypes
                    .Contains(model.Type));

            foreach (var affectedSkillGroup in skillGroups)
            {
                var attributeTypesForCalculation = RuleConstants.GetSkillGroupSources(affectedSkillGroup.Type);

                double tmp = 0;
                foreach (var attributeType in attributeTypesForCalculation)
                {
                    tmp += CharacterModel.Attributes.First(attribute => attribute.Type == attributeType).FinalValue;
                }

                var newBaseValue = (int) Math.Round((tmp / 6), MidpointRounding.AwayFromZero);

                affectedSkillGroup.BaseValue = newBaseValue;
                SkillExtensions.RecalculateFinalValue(affectedSkillGroup);

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
            foreach (var bodyPart in CharacterModel.BodyParts)
            {
                double constFinalValue = GetAttributeSum(AttributeType.Konstitution);

                switch (bodyPart.Type)
                {
                    case BodyPartType.Kopf:
                    {
                        var newValue = (constFinalValue / 15) + 3;
                        bodyPart.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case BodyPartType.Torso:
                    {
                        var newValue = (constFinalValue / 6) + 2;
                        bodyPart.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case BodyPartType.ArmLinks:
                    case BodyPartType.ArmRechts:
                    {
                        var newValue = (constFinalValue / 10) + 1;
                        bodyPart.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    case BodyPartType.BeinLinks:
                    case BodyPartType.BeinRechts:
                    {
                        var newValue = (constFinalValue / 7) + 2;
                        bodyPart.MaxHitpoints = (int) Math.Round(newValue, MidpointRounding.AwayFromZero);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //update handicap
            RecalculateHandicapAttributes();
        }

        public void RecalculateSpecialAttributes(params SpecialAttributeModel[] attributes)
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
            var fightingArmorLoad = CharacterModel.BodyParts.Sum(bodyPart =>
                bodyPart.Armor.Where(armor => armor.Fight).Sum(_ => _.LoadValue));
            var fightingWeaponLoad = CharacterModel.Weapons.Sum(weapon => weapon.LoadValue);
            var fightingItemLoad = CharacterModel.EquippedItems.Where(item => item.Fight).Sum(_ => _.LoadValue);
            return fightingArmorLoad + fightingWeaponLoad + fightingItemLoad;
        }

        private int GetAdventureLoad()
        {
            var adventureItemLoad = CharacterModel.EquippedItems.Where(item => item.Adventure).Sum(_ => _.LoadValue);
            var adventureArmorLoad = CharacterModel.BodyParts.Sum(bodyPart =>
                bodyPart.Armor.Where(armor => armor.Adventure).Sum(_ => _.LoadValue));
            var fightingWeaponLoad = CharacterModel.Weapons.Sum(weapon => weapon.LoadValue);
            return adventureArmorLoad + fightingWeaponLoad + adventureItemLoad;
        }

        private int GetCompleteLoad()
        {
            var itemLoad = CharacterModel.EquippedItems.Sum(_ => _.LoadValue);
            var armorLoad = CharacterModel.BodyParts.Sum(bodyPart => bodyPart.Armor.Sum(_ => _.LoadValue));
            var fightLoad = CharacterModel.Weapons.Sum(weapon => weapon.LoadValue);
            return armorLoad + fightLoad + itemLoad;
        }

        private double GetAttributeSum(params AttributeType[] attributes)
        {
            return attributes.Sum(attributeType => CharacterModel.Attributes.First(_ => _.Type == attributeType).FinalValue);
        }
        private double GetDerivedAttributeSum(params DerivedAttributeType[] attributes)
        {
            return attributes.Sum(attributeType => DerivedAttributes.First(_ => _.Type == attributeType).FinalValue);
        }
    }
}