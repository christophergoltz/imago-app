using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImagoApp.Repository.WrappingDatabase
{
    public interface ICharacterRepository : IObjectJsonRepository<Models.Character, Models.Entity.CharacterEntity>
    {
        Task EnsureTables();

        Models.Character CreateNewCharacter();

        Models.Character CreateExampleCharacter();
        Task<bool> Update(Models.Character character);
    }

    public class CharacterRepository : ObjectJsonRepositoryBase<Models.Character, Models.Entity.CharacterEntity>, ICharacterRepository
    {
        public CharacterRepository(string databaseFolder) : base(databaseFolder, "Imago_Character.db3")
        {
        }

        public async Task EnsureTables()
        {
            await Database.CreateTableAsync<Models.Entity.CharacterEntity>();
        }

        public async Task<bool> Update(Models.Character character)
        {
            var characterEntity = new Models.Entity.CharacterEntity()
            {
                Value = character,
                Name = character.Name,
                CreatedAt = character.CreatedAt,
                Id = character.Id,
                LastModifiedAt = character.LastModifiedAt,
                Version = character.Version
            };

            return (await Database.UpdateAsync(characterEntity)) == 1;
        }

        public Models.Character CreateNewCharacter()
        {
            var character = new Models.Character
            {
                Attributes = CreateAttributes(),
                SkillGroups = new Dictionary<Models.Enum.SkillGroupModelType, Models.SkillGroupModel>(),
                Id = Guid.NewGuid(),
                OpenAttributeIncreases = new ObservableCollection<Models.Enum.SkillGroupModelType>(),
                RaceType = Models.Enum.RaceType.Mensch,
                BodyParts = CreateBodyParts(),
                Weapons = new ObservableCollection<Models.Weapon>(),
                EquippedItems = new ObservableCollection<Models.EquipableItem>(),
                BloodCarrier = new ObservableCollection<Models.BloodCarrierModel>()
            };

            //add skillgroups
            character.SkillGroups.Add(Models.Enum.SkillGroupModelType.Nahkampf, CreateSkillGroups(Models.Enum.SkillGroupModelType.Nahkampf));
            character.SkillGroups.Add(Models.Enum.SkillGroupModelType.Heimlichkeit, CreateSkillGroups(Models.Enum.SkillGroupModelType.Heimlichkeit));
            character.SkillGroups.Add(Models.Enum.SkillGroupModelType.Fernkampf, CreateSkillGroups(Models.Enum.SkillGroupModelType.Fernkampf));
            character.SkillGroups.Add(Models.Enum.SkillGroupModelType.Bewegung, CreateSkillGroups(Models.Enum.SkillGroupModelType.Bewegung));
            character.SkillGroups.Add(Models.Enum.SkillGroupModelType.Handwerk, CreateSkillGroups(Models.Enum.SkillGroupModelType.Handwerk));
            character.SkillGroups.Add(Models.Enum.SkillGroupModelType.Soziales, CreateSkillGroups(Models.Enum.SkillGroupModelType.Soziales));
            character.SkillGroups.Add(Models.Enum.SkillGroupModelType.Webkunst, CreateSkillGroups(Models.Enum.SkillGroupModelType.Webkunst));
            character.SkillGroups.Add(Models.Enum.SkillGroupModelType.Wissenschaft, CreateSkillGroups(Models.Enum.SkillGroupModelType.Wissenschaft));

            return character;
        }

        public Models.Character CreateExampleCharacter()
        {
            var jsonValue =
                "{\"Name\":\"Testspieler\",\"RaceType\":1,\"Height\":\"178cm\",\"Weight\":\"93kg\",\"DivineSoul\":\"40\",\"EyeColor\":\"Blau\",\"HairColor\":\"Schwarz\",\"SkinColor\":\"Hell\",\"Age\":\"62\",\"Id\":\"9f99c7f1-50fd-411a-a5d6-2e5af182f436\",\"CreatedAt\":\"2021-06-21T12:09:23.928661+02:00\",\"LastModifiedAt\":\"2021-06-21T12:09:23.9287264+02:00\",\"CreatedBy\":\"System\",\"Owner\":\"Testuser\",\"FreeSkillExperience\":6,\"Attributes\":[{\"Type\":1,\"Corrosion\":0,\"NaturalValue\":10,\"TotalExperience\":300,\"ExperienceValue\":8,\"IncreaseValue\":66,\"ExperienceForNextIncreasedRequired\":12,\"ModificationValue\":0},{\"Type\":2,\"Corrosion\":0,\"NaturalValue\":-10,\"TotalExperience\":200,\"ExperienceValue\":4,\"IncreaseValue\":57,\"ExperienceForNextIncreasedRequired\":8,\"ModificationValue\":0},{\"Type\":3,\"Corrosion\":0,\"NaturalValue\":15,\"TotalExperience\":400,\"ExperienceValue\":9,\"IncreaseValue\":73,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":4,\"Corrosion\":0,\"NaturalValue\":0,\"TotalExperience\":350,\"ExperienceValue\":10,\"IncreaseValue\":70,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":5,\"Corrosion\":0,\"NaturalValue\":0,\"TotalExperience\":500,\"ExperienceValue\":7,\"IncreaseValue\":79,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":6,\"Corrosion\":0,\"NaturalValue\":0,\"TotalExperience\":320,\"ExperienceValue\":4,\"IncreaseValue\":68,\"ExperienceForNextIncreasedRequired\":12,\"ModificationValue\":0},{\"Type\":7,\"Corrosion\":0,\"NaturalValue\":-15,\"TotalExperience\":400,\"ExperienceValue\":9,\"IncreaseValue\":73,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0}],\"SkillGroups\":{\"Nahkampf\":{\"Type\":2,\"ModificationValue\":0,\"IncreaseValue\":58,\"ExperienceValue\":6,\"ExperienceForNextIncreasedRequired\":8,\"Skills\":[{\"Type\":11,\"TotalExperience\":450,\"ExperienceValue\":8,\"IncreaseValue\":76,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":17,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":32,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":35,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":44,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":52,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":57,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":210},\"Heimlichkeit\":{\"Type\":3,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":33,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":37,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":43,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":48,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":51,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":49,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":50,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Fernkampf\":{\"Type\":4,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":4,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":8,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":9,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":34,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":54,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":55,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Bewegung\":{\"Type\":1,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":6,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":19,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":20,\"TotalExperience\":360,\"ExperienceValue\":3,\"IncreaseValue\":71,\"ExperienceForNextIncreasedRequired\":17,\"ModificationValue\":0},{\"Type\":22,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":31,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":36,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":42,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":47,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Handwerk\":{\"Type\":7,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":1,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":18,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":28,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":41,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":45,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":56,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Soziales\":{\"Type\":8,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":3,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":5,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":13,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":15,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":16,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":25,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":38,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Webkunst\":{\"Type\":5,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":7,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":10,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":12,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":14,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":21,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":24,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":26,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":46,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0},\"Wissenschaft\":{\"Type\":6,\"ModificationValue\":0,\"IncreaseValue\":0,\"ExperienceValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"Skills\":[{\"Type\":2,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":23,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":27,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":29,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":30,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":39,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":40,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0},{\"Type\":53,\"TotalExperience\":0,\"ExperienceValue\":0,\"IncreaseValue\":0,\"ExperienceForNextIncreasedRequired\":0,\"ModificationValue\":0}],\"TotalExperience\":0}},\"OpenAttributeIncreases\":[],\"BodyParts\":{\"Kopf\":{\"Armor\":[],\"Type\":1,\"CurrentHitpoints\":7,\"MaxHitpoints\":9},\"Torso\":{\"Armor\":[],\"Type\":2,\"CurrentHitpoints\":12,\"MaxHitpoints\":17},\"ArmLinks\":{\"Armor\":[],\"Type\":3,\"CurrentHitpoints\":1,\"MaxHitpoints\":10},\"ArmRechts\":{\"Armor\":[],\"Type\":4,\"CurrentHitpoints\":8,\"MaxHitpoints\":10},\"BeinLinks\":{\"Armor\":[],\"Type\":5,\"CurrentHitpoints\":6,\"MaxHitpoints\":15},\"BeinRechts\":{\"Armor\":[],\"Type\":6,\"CurrentHitpoints\":3,\"MaxHitpoints\":15}},\"Weapons\":[{\"WeaponStances\":[{\"Type\":\"leichte Haltung\",\"PhaseValue\":\"4\",\"DamageFormula\":\"1W6+1P\",\"ParryModifier\":\"-45\",\"Range\":\"-\"},{\"Type\":\"schwere Haltung\",\"PhaseValue\":\"5\",\"DamageFormula\":\"2W6P\",\"ParryModifier\":\"-20\",\"Range\":\"-\"}],\"DurabilityValue\":35,\"Fight\":true,\"Adventure\":true,\"LoadValue\":40,\"Name\":\"Axt\"},{\"WeaponStances\":[{\"Type\":\"leichte Haltung\",\"PhaseValue\":\"2\",\"DamageFormula\":\"1W3P\",\"ParryModifier\":\"-55\",\"Range\":\"-\"},{\"Type\":\"schwere Haltung\",\"PhaseValue\":\"3\",\"DamageFormula\":\"1W3+1P\",\"ParryModifier\":\"-30\",\"Range\":\"-\"}],\"DurabilityValue\":25,\"Fight\":true,\"Adventure\":true,\"LoadValue\":5,\"Name\":\"Dolch\"}],\"EquippedItems\":[{\"Fight\":false,\"Adventure\":true,\"LoadValue\":20,\"Name\":\"Mantel\"},{\"Fight\":true,\"Adventure\":true,\"LoadValue\":5,\"Name\":\"Gürtel\"},{\"Fight\":false,\"Adventure\":false,\"LoadValue\":45,\"Name\":\"Heiler Material Stufe 2\"}],\"BloodCarrier\":[{\"Name\":\"Sakrament der Schmerzen\",\"CurrentCapacity\":25,\"MaximumCapacity\":30,\"Regeneration\":3}]}";

            return JsonConvert.DeserializeObject<Models.Character>(jsonValue);
        }

        private Dictionary<Models.Enum.BodyPartType, Models.BodyPart> CreateBodyParts()
        {
            return new Dictionary<Models.Enum.BodyPartType, Models.BodyPart>
            {
                {
                    Models.Enum.BodyPartType.Kopf,
                    new Models.BodyPart(Models.Enum.BodyPartType.Kopf, 7, new ObservableCollection<Models.ArmorPartModel>())
                },
                {
                    Models.Enum.BodyPartType.Torso,
                    new Models.BodyPart(Models.Enum.BodyPartType.Torso, 12, new ObservableCollection<Models.ArmorPartModel>())
                },
                {
                    Models.Enum.BodyPartType.ArmLinks,
                    new Models.BodyPart(Models.Enum.BodyPartType.ArmLinks, 1, new ObservableCollection<Models.ArmorPartModel>())
                },
                {
                    Models.Enum.BodyPartType.ArmRechts,
                    new Models.BodyPart(Models.Enum.BodyPartType.ArmRechts, 8, new ObservableCollection<Models.ArmorPartModel>())
                },
                {
                    Models.Enum.BodyPartType.BeinLinks,
                    new Models.BodyPart(Models.Enum.BodyPartType.BeinLinks, 6, new ObservableCollection<Models.ArmorPartModel>())
                },
                {
                    Models.Enum.BodyPartType.BeinRechts,
                    new Models.BodyPart(Models.Enum.BodyPartType.BeinRechts, 3, new ObservableCollection<Models.ArmorPartModel>())
                }
            };
        }

        private List<Models.Attribute> CreateAttributes()
        {
            return new List<Models.Attribute>
            {
                new Models.Attribute(Models.Enum.AttributeType.Staerke) ,
                new Models.Attribute(Models.Enum.AttributeType.Geschicklichkeit) ,
                new Models.Attribute(Models.Enum.AttributeType.Konstitution),
                new Models.Attribute(Models.Enum.AttributeType.Intelligenz) ,
                new Models.Attribute(Models.Enum.AttributeType.Willenskraft),
                new Models.Attribute(Models.Enum.AttributeType.Charisma) ,
                new Models.Attribute(Models.Enum.AttributeType.Wahrnehmung)
            };
        }

        private Models.SkillGroupModel CreateSkillGroups(Models.Enum.SkillGroupModelType modelType)
        {
            var skillGroup = new Models.SkillGroupModel(modelType)
            {
                Skills = CreateSkills(modelType)
            };

            return skillGroup;
        }

        private List<Models.SkillModel> CreateSkills(Models.Enum.SkillGroupModelType groupModelType)
        {
            switch (groupModelType)
            {
                case Models.Enum.SkillGroupModelType.Bewegung:
                    return new List<Models.SkillModel>
                    {
                        new Models.SkillModel(Models.SkillModelType.Ausweichen),
                        new Models.SkillModel(Models.SkillModelType.Klettern),
                        new Models.SkillModel(Models.SkillModelType.Koerperbeherrschung),
                        new Models.SkillModel(Models.SkillModelType.Laufen),
                        new Models.SkillModel(Models.SkillModelType.Reiten),
                        new Models.SkillModel(Models.SkillModelType.Schwimmen),
                        new Models.SkillModel(Models.SkillModelType.Springen),
                        new Models.SkillModel(Models.SkillModelType.Tanzen)
                    };
                case Models.Enum.SkillGroupModelType.Nahkampf:
                    return new List<Models.SkillModel>
                    {
                        new Models.SkillModel(Models.SkillModelType.Dolche) ,
                        new Models.SkillModel(Models.SkillModelType.Hiebwaffen),
                        new Models.SkillModel(Models.SkillModelType.Schilde),
                        new Models.SkillModel(Models.SkillModelType.Schwerter),
                        new Models.SkillModel(Models.SkillModelType.StaebeSpeere),
                        new Models.SkillModel(Models.SkillModelType.Waffenlos),
                        new Models.SkillModel(Models.SkillModelType.Zweihaender),
                    };
                case Models.Enum.SkillGroupModelType.Heimlichkeit:
                    return new List<Models.SkillModel>
                    {
                        new Models.SkillModel(Models.SkillModelType.Schleichen),
                        new Models.SkillModel(Models.SkillModelType.Sicherheit),
                        new Models.SkillModel(Models.SkillModelType.SpurenLesen),
                        new Models.SkillModel(Models.SkillModelType.Taschendiebstahl),
                        new Models.SkillModel(Models.SkillModelType.Verstecken),
                        new Models.SkillModel(Models.SkillModelType.Verbergen),
                        new Models.SkillModel(Models.SkillModelType.Verkleiden),
                    };
                case Models.Enum.SkillGroupModelType.Fernkampf:
                    return new List<Models.SkillModel>
                    {
                        new Models.SkillModel(Models.SkillModelType.Armbrueste),
                        new Models.SkillModel(Models.SkillModelType.Blasrohre),
                        new Models.SkillModel(Models.SkillModelType.Boegen),
                        new Models.SkillModel(Models.SkillModelType.Schleuder),
                        new Models.SkillModel(Models.SkillModelType.Wurfgeschosse),
                        new Models.SkillModel(Models.SkillModelType.Wurfwaffen),
                    };
                case Models.Enum.SkillGroupModelType.Webkunst:
                    return new List<Models.SkillModel>
                    {
                        new Models.SkillModel(Models.SkillModelType.Bewusstsein),
                        new Models.SkillModel(Models.SkillModelType.Chaos),
                        new Models.SkillModel(Models.SkillModelType.Einfalt),
                        new Models.SkillModel(Models.SkillModelType.Ekstase),
                        new Models.SkillModel(Models.SkillModelType.Kontrolle),
                        new Models.SkillModel(Models.SkillModelType.Leere),
                        new Models.SkillModel(Models.SkillModelType.Materie),
                        new Models.SkillModel(Models.SkillModelType.Struktur),
                    };
                case Models.Enum.SkillGroupModelType.Wissenschaft:
                    return new List<Models.SkillModel>
                    {
                        new Models.SkillModel(Models.SkillModelType.Anatomie),
                        new Models.SkillModel(Models.SkillModelType.Literatur),
                        new Models.SkillModel(Models.SkillModelType.Mathematik),
                        new Models.SkillModel(Models.SkillModelType.Philosophie),
                        new Models.SkillModel(Models.SkillModelType.Physik),
                        new Models.SkillModel(Models.SkillModelType.Soziologie),
                        new Models.SkillModel(Models.SkillModelType.Sphaerologie),
                        new Models.SkillModel(Models.SkillModelType.WirtschaftRecht),
                    };
                case Models.Enum.SkillGroupModelType.Handwerk:
                    return new List<Models.SkillModel>
                    {
                        new Models.SkillModel(Models.SkillModelType.Alchemie),
                        new Models.SkillModel(Models.SkillModelType.Heiler),
                        new Models.SkillModel(Models.SkillModelType.Naturkunde),
                        new Models.SkillModel(Models.SkillModelType.Sprache),
                        new Models.SkillModel(Models.SkillModelType.Strategie),
                        new Models.SkillModel(Models.SkillModelType.Wundscher),
                    };
                case Models.Enum.SkillGroupModelType.Soziales:
                    return new List<Models.SkillModel>
                    {
                        new Models.SkillModel(Models.SkillModelType.Anfuehren),
                        new Models.SkillModel(Models.SkillModelType.Ausdruck),
                        new Models.SkillModel(Models.SkillModelType.Einschuechtern),
                        new Models.SkillModel(Models.SkillModelType.Empathie),
                        new Models.SkillModel(Models.SkillModelType.Gesellschafter),
                        new Models.SkillModel(Models.SkillModelType.Manipulation),
                        new Models.SkillModel(Models.SkillModelType.SozialeAdaption),
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(groupModelType), groupModelType, null);
            }

        }
    }
}