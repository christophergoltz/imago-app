using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class CharacterViewModel : BindableBase, IInitialCalculationViewModel
    {
        private readonly IAttributeCalculationService _attributeCalculationService;
        private readonly ISkillGroupCalculationService _skillGroupCalculationService;
        private readonly ISkillCalculationService _skillCalculationService;
        public CharacterModel CharacterModel { get; }
        private bool _editMode;

        public bool EditMode
        {
            get => _editMode;
            set => SetProperty(ref _editMode, value);
        }

        public AttributeModel CharismaAttribute =>
            CharacterModel.Attributes.First(model => model.Type == AttributeType.Charisma);

        public AttributeModel GeschicklichkeitAttribute =>
            CharacterModel.Attributes.First(model => model.Type == AttributeType.Geschicklichkeit);

        public AttributeModel IntelligenzAttribute =>
            CharacterModel.Attributes.First(model => model.Type == AttributeType.Intelligenz);

        public AttributeModel KonstitutionAttribute =>
            CharacterModel.Attributes.First(model => model.Type == AttributeType.Konstitution);

        public AttributeModel StaerkeAttribute =>
            CharacterModel.Attributes.First(model => model.Type == AttributeType.Staerke);

        public AttributeModel WahrnehmungAttribute =>
            CharacterModel.Attributes.First(model => model.Type == AttributeType.Wahrnehmung);

        public AttributeModel WillenskraftAttribute =>
            CharacterModel.Attributes.First(model => model.Type == AttributeType.Willenskraft);

        public CharacterViewModel(CharacterModel characterModel,
            IAttributeCalculationService attributeCalculationService,
            ISkillGroupCalculationService skillGroupCalculationService,
            ISkillCalculationService skillCalculationService)
        {
            _attributeCalculationService = attributeCalculationService;
            _skillGroupCalculationService = skillGroupCalculationService;
            _skillCalculationService = skillCalculationService;
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

            SpecialAttributes = new List<SpecialAttributeModel>
            {
                new SpecialAttributeModel(SpecialAttributeType.Initiative)
            };
        }

        public List<SpecialAttributeModel> SpecialAttributes { get; }
        public List<DerivedAttributeModel> DerivedAttributes { get; }

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

        public void SetModification(SkillModel target, int modification)
        {
            Debug.WriteLine($"Set Modification for {target.Type} to {modification}");

            var finalValueChanged = _skillCalculationService.SetModification(target, modification);
            if (!finalValueChanged)
                return;
        }

        public void SetBaseValue(AttributeModel target, int baseValue)
        {
            Debug.WriteLine($"Set BaseValue for {target.Type} to {baseValue}");

            var finalValueChanged = _attributeCalculationService.SetBaseValue(target, baseValue, CharacterModel.Attributes, CharacterModel.SkillGroups);
            if (!finalValueChanged)
                return;

            ApplyNewFinalValueOfAttribute(target);
        }

        public void SetModification(SkillGroupModel target, int modification)
        {
            Debug.WriteLine($"Set Modification for {target.Type} to {modification}");

            var finalValueChanged = _skillGroupCalculationService.SetModification(target, modification);
            if (!finalValueChanged)
                return;
        }

        public void SetModificationValue(SpecialAttributeModel specialAttributeModel, int modificationValue)
        {
            Debug.WriteLine($"Set Modification for {specialAttributeModel.Type} to {modificationValue}");

            specialAttributeModel.ModificationValue = modificationValue;
            RecalculateSpecialAttributes(specialAttributeModel);
        }

        public bool CheckTalentRequirement(List<SkillRequirementModel> requirements)
        {
            foreach (var requirement in requirements)
            {
                var skill = CharacterModel.SkillGroups.SelectMany(pair => pair.Skills)
                    .First(_ => _.Type == requirement.Type);
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
                var skillGroup = CharacterModel.SkillGroups.First(_ => _.Type == requirement.Type);
                if (skillGroup.IncreaseValue < requirement.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public void SetCreationExperience(SkillModel target, SkillGroupModel parent, int experience)
        {
            Debug.WriteLine($"Set CreationExperience for {target.Type} to {experience}");

            var skillChange = _skillCalculationService.SetCreationExperience(target, experience);
            if (skillChange.IncreaseValueChange != 0)
            {
                //add exp to skillgroup
                var skillGroupChange = _skillGroupCalculationService.AddExperience(parent, skillChange.IncreaseValueChange);

                AddExperienceToSkillGroup(parent, skillGroupChange.IncreaseValueChange);
            }
        }

        public void AddExperienceToSkill(SkillModel target, SkillGroupModel parent, int experience)
        {
            Debug.WriteLine($"Add Experience for {target.Type} by {experience}");

            var skillChange = _skillCalculationService.AddExperience(target, experience);
            if (skillChange.IncreaseValueChange != 0)
            {
                AddExperienceToSkillGroup(parent, skillChange.IncreaseValueChange);
            }
        }

        private void AddExperienceToSkillGroup(SkillGroupModel skillGroup, int experience)
        {
            Debug.WriteLine($"Add Experience for {skillGroup.Type} by {experience}");

            //add exp to skillgroup
            var skillGroupChange = _skillGroupCalculationService.AddExperience(skillGroup, experience);
            var change = skillGroupChange.IncreaseValueChange;

            if (change > 0)
            {
                //add
                for (var i = 0; i < change; i++)
                {
                    CharacterModel.OpenAttributeIncreases.Add(skillGroup.Type);
                }
            }
            else if (change < 0)
            {
                //remove
                var leftOverExperienceToReduce = 0;
                for (var i = 0; i < change * -1; i++)
                {
                    if (CharacterModel.OpenAttributeIncreases.Contains(skillGroup.Type))
                        CharacterModel.OpenAttributeIncreases.Remove(skillGroup.Type);
                    else
                        leftOverExperienceToReduce++;
                }

                if (leftOverExperienceToReduce > 0)
                {
                    //todo find a better solution as throwing the error
                    //Cannot reduce spent attribute experience
                    UserDialogs.Instance.Alert(
                        $"Durch die SW-Reduzierung der Fertigkeitskategorie {skillGroup.Type} müssen die Erfahrungpunkt(e) eines Attributes um {leftOverExperienceToReduce} reduziert werden, " +
                        $"welche schon Ausgegeben wurde.{Environment.NewLine}{Environment.NewLine}Momentan wird dies nicht unterstüzt und die Erfahrungspunkte der Attribute weichen nun möglicherweise vom Regelwerk ab." +
                        "Ggf. müssen die Attributs-Erfahrungspunkte neu verteilt werden",
                        "Attributs-Erfahrung reduzieren");
                    Crashes.TrackError(new Exception("Unable to reduce attributeexperience"));
                }
            }
        }

        public void SetCreationExperienceToAttribute(AttributeModel target, int creationExperience)
        {
            Debug.WriteLine($"Set CreationEp for {target.Type} to {creationExperience}");

            var finalValueChanged = _attributeCalculationService.SetCreationExperience(target, creationExperience, CharacterModel.Attributes, CharacterModel.SkillGroups);
            if (!finalValueChanged)
                return;

            ApplyNewFinalValueOfAttribute(target);
        }

        public void AddSkillGroupExperienceToAttribute(AttributeModel target, int skillGroupExperienceChange)
        {
            Debug.WriteLine($"Add SkillGroupExperience for {target.Type} by {skillGroupExperienceChange}");

            var finalValueChanged = _attributeCalculationService.AddSkillGroupExperience(target, skillGroupExperienceChange, CharacterModel.Attributes, CharacterModel.SkillGroups);
            if (!finalValueChanged)
                return;

            ApplyNewFinalValueOfAttribute(target);
        }

        public void SetCorrosion(AttributeModel target, int corrosion)
        {
            Debug.WriteLine($"Set Corrosion for {target.Type} to {corrosion}");

            var finalValueChanged = _attributeCalculationService.SetCorrosion(target, corrosion, CharacterModel.Attributes, CharacterModel.SkillGroups);
            if (!finalValueChanged)
                return;

            ApplyNewFinalValueOfAttribute(target);
        }

        public void SetModification(AttributeModel target, int modification)
        {
            Debug.WriteLine($"Set Modification for {target.Type} to {modification}");

            var finalValueChanged = _attributeCalculationService.SetModification(target, modification, CharacterModel.Attributes, CharacterModel.SkillGroups);
            if (!finalValueChanged)
                return;

            ApplyNewFinalValueOfAttribute(target);
        }

        public void SetExperienceToAttribute(AttributeModel target, int experience)
        {
            Debug.WriteLine($"Set Experience for {target.Type} to {experience}");

            var finalValueChanged = _attributeCalculationService.SetExperience(target, experience, CharacterModel.Attributes, CharacterModel.SkillGroups);
            if (!finalValueChanged)
                return;

            ApplyNewFinalValueOfAttribute(target);
        }

        private void ApplyNewFinalValueOfAttribute(AttributeModel changedAttribute = null)
        {
            if (changedAttribute != null)
                Debug.WriteLine($"ApplyNewFinalValueOfAttribute {changedAttribute.Type}");

            //todo only recalc what is affected -> changedAttribute

            var constFinalValue = GetAttributeSum(AttributeType.Konstitution);
            var staerkeFinalValue = GetAttributeSum(AttributeType.Staerke);
            var willenskraftFinalValue = GetAttributeSum(AttributeType.Willenskraft);
            var geschicklichkeitFinalValue = GetAttributeSum(AttributeType.Geschicklichkeit);

            //update derived attributes
            RecalculateDerivedAttributes(willenskraftFinalValue, staerkeFinalValue, constFinalValue, geschicklichkeitFinalValue);

            //update special attributes
            RecalculateSpecialAttributes(SpecialAttributes.ToArray());

            //update bodyparts
            RecalculateBodyParts(constFinalValue);

            //update handicap
            RecalculateHandicapAttributes();
        }

        private void RecalculateDerivedAttributes(double willenskraftFinalValue, double staerkeFinalValue, double konstitutionFinalValue, double geschicklichkeitFinalValue)
        {
            foreach (var derivedAttribute in DerivedAttributes)
            {
                switch (derivedAttribute.Type)
                {
                    case DerivedAttributeType.Egoregenration:
                        {
                            var baseValue = willenskraftFinalValue / 5;
                            derivedAttribute.FinalValue = (int)Math.Round(baseValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                    case DerivedAttributeType.Schadensmod:
                        {
                            var baseValue = (staerkeFinalValue / 10) - 5;
                            derivedAttribute.FinalValue = (int)Math.Round(baseValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                    case DerivedAttributeType.Traglast:
                        {
                            var baseValue = (konstitutionFinalValue + konstitutionFinalValue + staerkeFinalValue) / 10;
                            derivedAttribute.FinalValue = (int)Math.Round(baseValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                    case DerivedAttributeType.TaktischeBewegung:
                        {
                            var baseValue = geschicklichkeitFinalValue / 10;
                            derivedAttribute.FinalValue = (int)Math.Round(baseValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                    case DerivedAttributeType.Sprintreichweite:
                        {
                            var baseValue = geschicklichkeitFinalValue / 5;
                            derivedAttribute.FinalValue = (int)Math.Round(baseValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                }
            }
        }

        private void RecalculateBodyParts(double constFinalValue)
        {
            foreach (var bodyPart in CharacterModel.BodyParts)
            {
                switch (bodyPart.Type)
                {
                    case BodyPartType.Kopf:
                        {
                            var newValue = (constFinalValue / 15) + 3;
                            bodyPart.MaxHitpoints = (int)Math.Round(newValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                    case BodyPartType.Torso:
                        {
                            var newValue = (constFinalValue / 6) + 2;
                            bodyPart.MaxHitpoints = (int)Math.Round(newValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                    case BodyPartType.ArmLinks:
                    case BodyPartType.ArmRechts:
                        {
                            var newValue = (constFinalValue / 10) + 1;
                            bodyPart.MaxHitpoints = (int)Math.Round(newValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                    case BodyPartType.BeinLinks:
                    case BodyPartType.BeinRechts:
                        {
                            var newValue = (constFinalValue / 7) + 2;
                            bodyPart.MaxHitpoints = (int)Math.Round(newValue, MidpointRounding.AwayFromZero);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(BodyPartType));
                }
            }
        }

        private void RecalculateSpecialAttributes(params SpecialAttributeModel[] attributes)
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
                            specialAttribute.FinalValue = ((int)Math.Round(newValue, MidpointRounding.AwayFromZero)) +
                                                          specialAttribute.ModificationValue;
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
                                (int)Math.Round(load / loadLimit, MidpointRounding.AwayFromZero);
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

        public void CalculateInitialValues()
        {
            _attributeCalculationService.RecalculateAllAttributes(CharacterModel.Attributes, CharacterModel.SkillGroups);
            ApplyNewFinalValueOfAttribute();
        }
    }
}