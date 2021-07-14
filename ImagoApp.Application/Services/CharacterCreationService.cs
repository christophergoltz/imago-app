using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;
using Newtonsoft.Json;

namespace ImagoApp.Application.Services
{
    public interface ICharacterCreationService
    {
        CharacterModel CreateNewCharacter();
        CharacterModel CreateExampleCharacter();
    }

    public class CharacterCreationService : ICharacterCreationService
    {
        public CharacterModel CreateNewCharacter()
        {
            var character = new CharacterModel
            {
                Attributes = CreateAttributes(),
                SkillGroups = new List<SkillGroupModel>(),
                Guid = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                OpenAttributeIncreases = new ObservableCollection<SkillGroupModelType>(),
                RaceType = RaceType.Mensch,
                BodyParts = CreateBodyParts(),
                Weapons = new ObservableCollection<WeaponModel>(),
                EquippedItems = new ObservableCollection<EquipableItemModel>(),
                BloodCarrier = new ObservableCollection<BloodCarrierModel>()
            };

            //add skillgroups
            character.SkillGroups.Add(CreateSkillGroups(SkillGroupModelType.Nahkampf));
            character.SkillGroups.Add( CreateSkillGroups(SkillGroupModelType.Heimlichkeit));
            character.SkillGroups.Add( CreateSkillGroups(SkillGroupModelType.Fernkampf));
            character.SkillGroups.Add(CreateSkillGroups(SkillGroupModelType.Bewegung));
            character.SkillGroups.Add(CreateSkillGroups(SkillGroupModelType.Handwerk));
            character.SkillGroups.Add(CreateSkillGroups(SkillGroupModelType.Soziales));
            character.SkillGroups.Add(CreateSkillGroups(SkillGroupModelType.Webkunst));
            character.SkillGroups.Add( CreateSkillGroups(SkillGroupModelType.Wissenschaft));

            return character;
        }

        public CharacterModel CreateExampleCharacter()
        {
            var character = CreateNewCharacter();
            character.Attributes.First(attribute => attribute.Type == AttributeType.Charisma).TotalExperience = 260;

            return character;
        }

        private List<BodyPartModel> CreateBodyParts()
        {
            return new List<BodyPartModel>
            {
                new BodyPartModel(BodyPartType.Kopf, 7, new ObservableCollection<ArmorPartModelModel>()),
                new BodyPartModel(BodyPartType.Torso, 12, new ObservableCollection<ArmorPartModelModel>()),
                new BodyPartModel(BodyPartType.ArmLinks, 1, new ObservableCollection<ArmorPartModelModel>()),
                new BodyPartModel(BodyPartType.ArmRechts, 8, new ObservableCollection<ArmorPartModelModel>()),
                new BodyPartModel(BodyPartType.BeinLinks, 6, new ObservableCollection<ArmorPartModelModel>()),
                new BodyPartModel(BodyPartType.BeinRechts, 3, new ObservableCollection<ArmorPartModelModel>())
            };
        }

        private List<AttributeModel> CreateAttributes()
        {
            return new List<AttributeModel>
            {
                new AttributeModel(AttributeType.Staerke) ,
                new AttributeModel(AttributeType.Geschicklichkeit) ,
                new AttributeModel(AttributeType.Konstitution),
                new AttributeModel(AttributeType.Intelligenz) ,
                new AttributeModel(AttributeType.Willenskraft),
                new AttributeModel(AttributeType.Charisma) ,
                new AttributeModel(AttributeType.Wahrnehmung)
            };
        }

        private SkillGroupModel CreateSkillGroups(SkillGroupModelType modelType)
        {
            var skillGroup = new SkillGroupModel(modelType)
            {
                Skills = CreateSkills(modelType)
            };

            return skillGroup;
        }

        private List<SkillModel> CreateSkills(SkillGroupModelType groupModelType)
        {
            switch (groupModelType)
            {
                case SkillGroupModelType.Bewegung:
                    return new List<SkillModel>
                    {
                        new SkillModel(SkillModelType.Ausweichen),
                        new SkillModel(SkillModelType.Klettern),
                        new SkillModel(SkillModelType.Koerperbeherrschung),
                        new SkillModel(SkillModelType.Laufen),
                        new SkillModel(SkillModelType.Reiten),
                        new SkillModel(SkillModelType.Schwimmen),
                        new SkillModel(SkillModelType.Springen),
                        new SkillModel(SkillModelType.Tanzen)
                    };
                case SkillGroupModelType.Nahkampf:
                    return new List<SkillModel>
                    {
                        new SkillModel(SkillModelType.Dolche) ,
                        new SkillModel(SkillModelType.Hiebwaffen),
                        new SkillModel(SkillModelType.Schilde),
                        new SkillModel(SkillModelType.Schwerter),
                        new SkillModel(SkillModelType.StaebeSpeere),
                        new SkillModel(SkillModelType.Waffenlos),
                        new SkillModel(SkillModelType.Zweihaender),
                    };
                case SkillGroupModelType.Heimlichkeit:
                    return new List<SkillModel>
                    {
                        new SkillModel(SkillModelType.Schleichen),
                        new SkillModel(SkillModelType.Sicherheit),
                        new SkillModel(SkillModelType.SpurenLesen),
                        new SkillModel(SkillModelType.Taschendiebstahl),
                        new SkillModel(SkillModelType.Verstecken),
                        new SkillModel(SkillModelType.Verbergen),
                        new SkillModel(SkillModelType.Verkleiden),
                    };
                case SkillGroupModelType.Fernkampf:
                    return new List<SkillModel>
                    {
                        new SkillModel(SkillModelType.Armbrueste),
                        new SkillModel(SkillModelType.Blasrohre),
                        new SkillModel(SkillModelType.Boegen),
                        new SkillModel(SkillModelType.Schleuder),
                        new SkillModel(SkillModelType.Wurfgeschosse),
                        new SkillModel(SkillModelType.Wurfwaffen),
                    };
                case SkillGroupModelType.Webkunst:
                    return new List<SkillModel>
                    {
                        new SkillModel(SkillModelType.Bewusstsein),
                        new SkillModel(SkillModelType.Chaos),
                        new SkillModel(SkillModelType.Einfalt),
                        new SkillModel(SkillModelType.Ekstase),
                        new SkillModel(SkillModelType.Kontrolle),
                        new SkillModel(SkillModelType.Leere),
                        new SkillModel(SkillModelType.Materie),
                        new SkillModel(SkillModelType.Struktur),
                    };
                case SkillGroupModelType.Wissenschaft:
                    return new List<SkillModel>
                    {
                        new SkillModel(SkillModelType.Anatomie),
                        new SkillModel(SkillModelType.Literatur),
                        new SkillModel(SkillModelType.Mathematik),
                        new SkillModel(SkillModelType.Philosophie),
                        new SkillModel(SkillModelType.Physik),
                        new SkillModel(SkillModelType.Soziologie),
                        new SkillModel(SkillModelType.Sphaerologie),
                        new SkillModel(SkillModelType.WirtschaftRecht),
                    };
                case SkillGroupModelType.Handwerk:
                    return new List<SkillModel>
                    {
                        new SkillModel(SkillModelType.Alchemie),
                        new SkillModel(SkillModelType.Heiler),
                        new SkillModel(SkillModelType.Naturkunde),
                        new SkillModel(SkillModelType.Sprache),
                        new SkillModel(SkillModelType.Strategie),
                        new SkillModel(SkillModelType.Wundscher),
                    };
                case SkillGroupModelType.Soziales:
                    return new List<SkillModel>
                    {
                        new SkillModel(SkillModelType.Anfuehren),
                        new SkillModel(SkillModelType.Ausdruck),
                        new SkillModel(SkillModelType.Einschuechtern),
                        new SkillModel(SkillModelType.Empathie),
                        new SkillModel(SkillModelType.Gesellschafter),
                        new SkillModel(SkillModelType.Manipulation),
                        new SkillModel(SkillModelType.SozialeAdaption),
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(groupModelType), groupModelType, null);
            }

        }
    }
}
