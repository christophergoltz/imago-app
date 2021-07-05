using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;
using Newtonsoft.Json;
using Attribute = ImagoApp.Application.Models.Attribute;

namespace ImagoApp.Application.Services
{
    public interface ICharacterCreationService
    {
        Character CreateNewCharacter();
        Character CreateExampleCharacter();
    }

    public class CharacterCreationService : ICharacterCreationService
    {
        public Character CreateNewCharacter()
        {
            var character = new Character
            {
                Attributes = CreateAttributes(),
                SkillGroups = new List<SkillGroupModel>(),
                Guid = Guid.NewGuid(),
                OpenAttributeIncreases = new ObservableCollection<SkillGroupModelType>(),
                RaceType = RaceType.Mensch,
                BodyParts = CreateBodyParts(),
                Weapons = new ObservableCollection<Weapon>(),
                EquippedItems = new ObservableCollection<EquipableItem>(),
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

        public Character CreateExampleCharacter()
        {
            var jsonValue =
                "{\"Name\":\"Testspieler\",\"RaceType\":1,\"Height\":\"178cm\",\"Weight\":\"93kg\",\"DivineSoul\":\"40\",\"EyeColor\":\"Blau\",\"HairColor\":\"Schwarz\",\"SkinColor\":\"Hell\",\"Age\":\"62\",\"Id\":\"9f99c7f1-50fd-411a-a5d6-2e5af182f436\",\"CreatedAt\":\"2021-06-21T12:09:23.928661+02:00\",\"LastModifiedAt\":\"2021-06-21T12:09:23.9287264+02:00\",\"CreatedBy\":\"System\",\"Owner\":\"Testuser\",\"FreeSkillExperience\":6,\"Attributes\":[{\"Type\":1,\"Corrosion\":0,\"NaturalValue\":10,\"TotalExperience\":300,\"ExperienceValue\":8,\"IncreaseValue\":66,\"ExperienceForNextIncreasedRequired\":12,\"ModificationValue\":0},{\"Type\":2,\"Corrosion\":0,\"NaturalValue\":-10,\"TotalExperience\":200,\"ExperienceValue\":4,\"IncreaseValue\":57,\"ExperienceForNextIncreasedRequired\":8,\"ModificationValue\":0},{\"Type\":3,\"Corrosion\":0,\"NaturalValue\":15,\"TotalExperience\":400,\"ExperienceValue\":9,\"IncreaseValue\":73,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":4,\"Corrosion\":0,\"NaturalValue\":0,\"TotalExperience\":350,\"ExperienceValue\":10,\"IncreaseValue\":70,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":5,\"Corrosion\":0,\"NaturalValue\":0,\"TotalExperience\":500,\"ExperienceValue\":7,\"IncreaseValue\":79,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":6,\"Corrosion\":0,\"NaturalValue\":0,\"TotalExperience\":320,\"ExperienceValue\":4,\"IncreaseValue\":68,\"ExperienceForNextIncreasedRequired\":12,\"ModificationValue\":0},{\"Type\":7,\"Corrosion\":0,\"NaturalValue\":-15,\"TotalExperience\":400,\"ExperienceValue\":9,\"IncreaseValue\":73,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0}],\"SkillGroups\":{\"Nahkampf\":{\"Type\":2,\"ModificationValue\":0,\"IncreaseValue\":58,\"ExperienceValue\":6,\"ExperienceForNextIncreasedRequired\":8,\"Skills\":[{\"Type\":11,\"TotalExperience\":450,\"ExperienceValue\":8,\"IncreaseValue\":76,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":17,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":32,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":35,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":44,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":52,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":57,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":210},\"Heimlichkeit\":{\"Type\":3,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":33,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":37,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":43,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":48,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":51,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":49,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":50,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Fernkampf\":{\"Type\":4,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":4,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":8,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":9,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":34,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":54,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":55,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Bewegung\":{\"Type\":1,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":6,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":19,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":20,\"TotalExperience\":360,\"ExperienceValue\":3,\"IncreaseValue\":71,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":22,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":31,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":36,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":42,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":47,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Handwerk\":{\"Type\":7,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":1,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":18,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":28,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":41,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":45,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":56,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Soziales\":{\"Type\":8,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":3,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":5,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":13,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":15,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":16,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":25,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":38,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Webkunst\":{\"Type\":5,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":7,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":10,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":12,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":14,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":21,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":24,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":26,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":46,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Wissenschaft\":{\"Type\":6,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":2,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":23,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":27,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":29,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":30,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":39,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":40,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":53,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0}},\"OpenAttributeIncreases\":[],\"BodyParts\":{\"Kopf\":{\"Armor\":[],\"Type\":1,\"CurrentHitpoints\":7,\"MaxHitpoints\":9},\"Torso\":{\"Armor\":[],\"Type\":2,\"CurrentHitpoints\":12,\"MaxHitpoints\":17},\"ArmLinks\":{\"Armor\":[],\"Type\":3,\"CurrentHitpoints\":1,\"MaxHitpoints\":10},\"ArmRechts\":{\"Armor\":[],\"Type\":4,\"CurrentHitpoints\":8,\"MaxHitpoints\":10},\"BeinLinks\":{\"Armor\":[],\"Type\":5,\"CurrentHitpoints\":6,\"MaxHitpoints\":15},\"BeinRechts\":{\"Armor\":[],\"Type\":6,\"CurrentHitpoints\":3,\"MaxHitpoints\":15}},\"Weapons\":[{\"WeaponStances\":[{\"Type\":\"leichte Haltung\",\"PhaseValue\":\"4\",\"DamageFormula\":\"1W6+1P\",\"ParryModifier\":\"-45\",\"Range\":\"-\"},{\"Type\":\"schwere Haltung\",\"PhaseValue\":\"5\",\"DamageFormula\":\"2W6P\",\"ParryModifier\":\"-20\",\"Range\":\"-\"}],\"DurabilityValue\":35,\"Fight\":true,\"Adventure\":true,\"LoadValue\":40,\"Name\":\"Axt\"},{\"WeaponStances\":[{\"Type\":\"leichte Haltung\",\"PhaseValue\":\"2\",\"DamageFormula\":\"1W3P\",\"ParryModifier\":\"-55\",\"Range\":\"-\"},{\"Type\":\"schwere Haltung\",\"PhaseValue\":\"3\",\"DamageFormula\":\"1W3+1P\",\"ParryModifier\":\"-30\",\"Range\":\"-\"}],\"DurabilityValue\":25,\"Fight\":true,\"Adventure\":true,\"LoadValue\":5,\"Name\":\"Dolch\"}],\"EquippedItems\":[{\"Fight\":false,\"Adventure\":true,\"LoadValue\":20,\"Name\":\"Mantel\"},{\"Fight\":true,\"Adventure\":true,\"LoadValue\":5,\"Name\":\"Gürtel\"},{\"Fight\":false,\"Adventure\":false,\"LoadValue\":45,\"Name\":\"Heiler Material Stufe 2\"}],\"BloodCarrier\":[{\"Name\":\"Sakrament der Schmerzen\",\"CurrentCapacity\":25,\"MaximumCapacity\":30,\"Regeneration\":3}]}";

            return JsonConvert.DeserializeObject<Character>(jsonValue);
        }

        private List<BodyPart> CreateBodyParts()
        {
            return new List<BodyPart>
            {
                new BodyPart(BodyPartType.Kopf, 7, new ObservableCollection<ArmorPartModel>()),
                new BodyPart(BodyPartType.Torso, 12, new ObservableCollection<ArmorPartModel>()),
                new BodyPart(BodyPartType.ArmLinks, 1, new ObservableCollection<ArmorPartModel>()),
                new BodyPart(BodyPartType.ArmRechts, 8, new ObservableCollection<ArmorPartModel>()),
                new BodyPart(BodyPartType.BeinLinks, 6, new ObservableCollection<ArmorPartModel>()),
                new BodyPart(BodyPartType.BeinRechts, 3, new ObservableCollection<ArmorPartModel>())
            };
        }

        private List<Attribute> CreateAttributes()
        {
            return new List<Attribute>
            {
                new Attribute(AttributeType.Staerke) ,
                new Attribute(AttributeType.Geschicklichkeit) ,
                new Attribute(AttributeType.Konstitution),
                new Attribute(AttributeType.Intelligenz) ,
                new Attribute(AttributeType.Willenskraft),
                new Attribute(AttributeType.Charisma) ,
                new Attribute(AttributeType.Wahrnehmung)
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
