using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Attribute = Imago.Models.Attribute;

namespace Imago.Repository
{
    public interface ICharacterRepository
    {
        Character CreateNewCharacter();

        Character CreateExampleCharacter();
    }

    public class CharacterRepository : ICharacterRepository
    {
        public Character CreateNewCharacter()
        {
            var character = new Character
            {
                Attributes = CreateAttributes(),
                SkillGroups = new Dictionary<SkillGroupType, SkillGroup>(),
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                OpenAttributeIncreases = new List<SkillGroupType>(),
                RaceType = RaceType.Mensch,
                BodyParts = CreateBodyParts(),
                Weapons = new ObservableCollection<Weapon>(),
                EquippedItems = CreateEquippedItems(),
                FreeSkillExperience = 6,
                BloodCarrier = new ObservableCollection<BloodCarrierModel>()
                    {new BloodCarrierModel("Sakrament der Schmerzen", 25, 30, 3)}
            };

            //add skillgroups
            character.SkillGroups.Add(SkillGroupType.Nahkampf, CreateSkillGroups(SkillGroupType.Nahkampf));
            character.SkillGroups.Add(SkillGroupType.Heimlichkeit, CreateSkillGroups(SkillGroupType.Heimlichkeit));
            character.SkillGroups.Add(SkillGroupType.Fernkampf, CreateSkillGroups(SkillGroupType.Fernkampf));
            character.SkillGroups.Add(SkillGroupType.Bewegung, CreateSkillGroups(SkillGroupType.Bewegung));
            character.SkillGroups.Add(SkillGroupType.Handwerk, CreateSkillGroups(SkillGroupType.Handwerk));
            character.SkillGroups.Add(SkillGroupType.Soziales, CreateSkillGroups(SkillGroupType.Soziales));
            character.SkillGroups.Add(SkillGroupType.Webkunst, CreateSkillGroups(SkillGroupType.Webkunst));
            character.SkillGroups.Add(SkillGroupType.Wissenschaft, CreateSkillGroups(SkillGroupType.Wissenschaft));

            return character;
        }

        public Character CreateExampleCharacter()
        {
            var character = new Character
            {
                Attributes = CreateAttributes(),
                SkillGroups = new Dictionary<SkillGroupType, SkillGroup>(),
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                OpenAttributeIncreases =
                    new List<SkillGroupType>(), // {SkillGroupType.Bewegung, SkillGroupType.Fernkampf},
                Age = "62",
                EyeColor = "Blau",
                HairColor = "Schwarz",
                RaceType = RaceType.Mensch,
                Height = "178cm",
                Name = "Klaus",
                Owner = "System",
                Weight = "93kg",
                SkinColor = "Hell",
                CreatedBy = "Testuser",
                DivineSoul = "40",
                BodyParts = CreateBodyParts(),
                Weapons = new ObservableCollection<Weapon>(),
                EquippedItems = CreateEquippedItems(),
                FreeSkillExperience = 6,
                BloodCarrier = new ObservableCollection<BloodCarrierModel>()
                    {new BloodCarrierModel("Sakrament der Schmerzen", 25, 30, 3)}
            };

            //add skillgroups
            character.SkillGroups.Add(SkillGroupType.Nahkampf, CreateSkillGroups(SkillGroupType.Nahkampf));
            character.SkillGroups.Add(SkillGroupType.Heimlichkeit, CreateSkillGroups(SkillGroupType.Heimlichkeit));
            character.SkillGroups.Add(SkillGroupType.Fernkampf, CreateSkillGroups(SkillGroupType.Fernkampf));
            character.SkillGroups.Add(SkillGroupType.Bewegung, CreateSkillGroups(SkillGroupType.Bewegung));
            character.SkillGroups.Add(SkillGroupType.Handwerk, CreateSkillGroups(SkillGroupType.Handwerk));
            character.SkillGroups.Add(SkillGroupType.Soziales, CreateSkillGroups(SkillGroupType.Soziales));
            character.SkillGroups.Add(SkillGroupType.Webkunst, CreateSkillGroups(SkillGroupType.Webkunst));
            character.SkillGroups.Add(SkillGroupType.Wissenschaft, CreateSkillGroups(SkillGroupType.Wissenschaft));

            return character;
        }

        private Dictionary<BodyPartType, BodyPart> CreateBodyParts()
        {
            return new Dictionary<BodyPartType, BodyPart>
            {
                {
                    BodyPartType.Kopf,
                    new BodyPart(BodyPartType.Kopf, "KO/15+3", 7, new ObservableCollection<ArmorModel>())
                },
                {
                    BodyPartType.Torso,
                    new BodyPart(BodyPartType.Torso, "KO/6+2", 12, new ObservableCollection<ArmorModel>())
                },
                {
                    BodyPartType.ArmLinks,
                    new BodyPart(BodyPartType.ArmLinks, "KO/10+1", 1, new ObservableCollection<ArmorModel>())
                },
                {
                    BodyPartType.ArmRechts,
                    new BodyPart(BodyPartType.ArmRechts, "KO/10+1", 8, new ObservableCollection<ArmorModel>())
                },
                {
                    BodyPartType.BeinLinks,
                    new BodyPart(BodyPartType.BeinLinks, "KO/7+2", 6, new ObservableCollection<ArmorModel>())
                },
                {
                    BodyPartType.BeinRechts,
                    new BodyPart(BodyPartType.BeinRechts, "KO/7+2", 3, new ObservableCollection<ArmorModel>())
                }
            };
        }
        
        public ObservableCollection<EquipableItem> CreateEquippedItems()
        {
            return new ObservableCollection<EquipableItem>()
            {
                new EquipableItem("Mantel",20, true, false),
                new EquipableItem("Gürtel", 5, true, true),
                new EquipableItem("Heiler Material Stufe 2", 45, false, false)
            };
        }

        private List<Attribute> CreateAttributes()
        {
            return new List<Attribute>
            {
                new Attribute(AttributeType.Staerke) {IncreaseValue = 50, NaturalValue = 10},
                new Attribute(AttributeType.Geschicklichkeit) {IncreaseValue = 45, NaturalValue = -10},
                new Attribute(AttributeType.Konstitution) {IncreaseValue = 51, NaturalValue = 15},
                new Attribute(AttributeType.Intelligenz) {IncreaseValue = 39},
                new Attribute(AttributeType.Willenskraft) {IncreaseValue = 52},
                new Attribute(AttributeType.Charisma) {IncreaseValue = 46},
                new Attribute(AttributeType.Wahrnehmung) {IncreaseValue = 50, NaturalValue = -15}
            };
        }

        private SkillGroup CreateSkillGroups(SkillGroupType type)
        {
            var skillGroup = new SkillGroup(type)
            {
                Skills = CreateSkills(type)
            };

            if (type == SkillGroupType.Nahkampf)
                skillGroup.IncreaseValue = 21;

            return skillGroup;
        }

        public List<Skill> CreateSkills(SkillGroupType groupType)
        {
            switch (groupType)
            {
                case SkillGroupType.Bewegung:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Ausweichen),
                        new Skill(SkillType.Klettern),
                        new Skill(SkillType.Koerperbeherrschung) {IncreaseValue = 36},
                        new Skill(SkillType.Laufen),
                        new Skill(SkillType.Reiten),
                        new Skill(SkillType.Schwimmen),
                        new Skill(SkillType.Springen),
                        new Skill(SkillType.Tanzen)
                    };
                case SkillGroupType.Nahkampf:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Dolche) {IncreaseValue = 66},
                        new Skill(SkillType.Hiebwaffen),
                        new Skill(SkillType.Schilde),
                        new Skill(SkillType.Schwerter),
                        new Skill(SkillType.StaebeSpeere),
                        new Skill(SkillType.Waffenlos),
                        new Skill(SkillType.Zweihaender),
                    };
                case SkillGroupType.Heimlichkeit:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Schleichen),
                        new Skill(SkillType.Sicherheit),
                        new Skill(SkillType.SpurenLesen),
                        new Skill(SkillType.Taschendiebstahl),
                        new Skill(SkillType.Verstecken),
                        new Skill(SkillType.Verbergen),
                        new Skill(SkillType.Verkleiden),
                    };
                case SkillGroupType.Fernkampf:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Armbrueste),
                        new Skill(SkillType.Blasrohre),
                        new Skill(SkillType.Boegen),
                        new Skill(SkillType.Schleuder),
                        new Skill(SkillType.Wurfgeschosse),
                        new Skill(SkillType.Wurfwaffen),
                    };
                case SkillGroupType.Webkunst:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Bewusstsein),
                        new Skill(SkillType.Chaos),
                        new Skill(SkillType.Einfalt),
                        new Skill(SkillType.Ekstase),
                        new Skill(SkillType.Kontrolle),
                        new Skill(SkillType.Leere),
                        new Skill(SkillType.Materie),
                        new Skill(SkillType.Struktur),
                    };
                case SkillGroupType.Wissenschaft:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Anatomie),
                        new Skill(SkillType.Literatur),
                        new Skill(SkillType.Mathematik),
                        new Skill(SkillType.Philosophie),
                        new Skill(SkillType.Physik),
                        new Skill(SkillType.Soziologie),
                        new Skill(SkillType.Sphaerologie),
                        new Skill(SkillType.WirtschaftRecht),
                    };
                case SkillGroupType.Handwerk:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Alchemie),
                        new Skill(SkillType.Heiler),
                        new Skill(SkillType.Naturkunde),
                        new Skill(SkillType.Sprache),
                        new Skill(SkillType.Strategie),
                        new Skill(SkillType.Wundscher),
                    };
                case SkillGroupType.Soziales:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Anfuehren),
                        new Skill(SkillType.Ausdruck),
                        new Skill(SkillType.Einschuechtern),
                        new Skill(SkillType.Empathie),
                        new Skill(SkillType.Gesellschafter),
                        new Skill(SkillType.Manipulation),
                        new Skill(SkillType.SozialeAdaption),
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(groupType), groupType, null);
            }

        }
    }
}