using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Attribute = Imago.Models.Attribute;

namespace Imago.Repository
{
    public interface ICharacterRepository
    {
        Character CreateNewCharacter();
    }

    public class CharacterRepository : ICharacterRepository
    {
        public Character CreateNewCharacter()
        {
            return new Character()
            {
                Attributes = CreateAttributes(),
                Nahkampf = CreateSkillGroups(SkillGroupType.Nahkampf),
                Heimlichkeit = CreateSkillGroups(SkillGroupType.Heimlichkeit),
                Fernkampf = CreateSkillGroups( SkillGroupType.Fernkampf),
                Bewegung = CreateSkillGroups(SkillGroupType.Bewegung),
                Handwerk = CreateSkillGroups(SkillGroupType.Handwerk),
                Soziales = CreateSkillGroups(SkillGroupType.Soziales),
                Webkunst = CreateSkillGroups(SkillGroupType.Webkunst),
                Wissenschaft = CreateSkillGroups(SkillGroupType.Wissenschaft),
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                GameVersion = new Version(1,0),
                Professions = CreateProfessions(),
                OpenAttributeIncreases = new List<SkillGroupType>(),
                Age = "62",
                EyeColor = "Blau",
                HairColor = "Schwarz",
                RaceType = RaceType.Mensch,
                Height = "178cm",
                Name = "Klaus",
                Owner = "System",
                Weight = "93kg",
                SkinColor = "Hell",
                CreatedBy = "Testuser"
            };
        }

        private List<Profession> CreateProfessions()
        {
            return new List<Profession>
            {
                new Profession(ProfessionType.Initiative, "(GE+GE+WA+WI)/4",
                    new List<AttributeType>
                    {
                        AttributeType.Geschicklichkeit, AttributeType.Willenskraft, AttributeType.Wahrnehmung
                    }),
                new Profession(ProfessionType.SchadensModifikator,"(ST/10)-5", new List<AttributeType> {AttributeType.Staerke}),
                new Profession(ProfessionType.EgoRegeneration, "WI/5",new List<AttributeType>() {AttributeType.Willenskraft}),
                new Profession(ProfessionType.LastGrenze, "(KO+ST+ST)/10",new List<AttributeType>(){AttributeType.Konstitution, AttributeType.Staerke}),
            };
        }

        private List<Attribute> CreateAttributes()
        {
            return new List<Attribute>
            {
                new Attribute(AttributeType.Staerke),
                new Attribute(AttributeType.Geschicklichkeit),
                new Attribute(AttributeType.Konstitution),
                new Attribute(AttributeType.Intelligenz),
                new Attribute(AttributeType.Willenskraft),
                new Attribute(AttributeType.Charisma),
                new Attribute(AttributeType.Wahrnehmung)
            };
        }
        
        private SkillGroup CreateSkillGroups(SkillGroupType type)
        {
            var skillGroup = new SkillGroup(type)
            {
                Skills = CreateSkills(type)
            };

            switch (type)
            {
                case SkillGroupType.Bewegung:
                    skillGroup.SkillSource = new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Konstitution, AttributeType.Konstitution };
                    break;
                case SkillGroupType.Nahkampf:
                    skillGroup.SkillSource = new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Konstitution, AttributeType.Wahrnehmung };
                    break;
                case SkillGroupType.Heimlichkeit:
                    skillGroup.SkillSource = new List<AttributeType> { AttributeType.Geschicklichkeit, AttributeType.Intelligenz, AttributeType.Willenskraft, AttributeType.Wahrnehmung };
                    break;
                case SkillGroupType.Fernkampf:
                    skillGroup.SkillSource = new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Geschicklichkeit, AttributeType.Wahrnehmung };
                    break;
                case SkillGroupType.Webkunst:
                    skillGroup.SkillSource = new List<AttributeType> { AttributeType.Willenskraft, AttributeType.Willenskraft, AttributeType.Charisma, AttributeType.Charisma };
                    break;
                case SkillGroupType.Wissenschaft:
                    skillGroup.SkillSource = new List<AttributeType> { AttributeType.Intelligenz, AttributeType.Intelligenz, AttributeType.Intelligenz, AttributeType.Wahrnehmung };
                    break;
                case SkillGroupType.Handwerk:
                    skillGroup.SkillSource = new List<AttributeType> { AttributeType.Geschicklichkeit, AttributeType.Intelligenz, AttributeType.Charisma, AttributeType.Wahrnehmung };
                    break;
                case SkillGroupType.Soziales:
                    skillGroup.SkillSource = new List<AttributeType> { AttributeType.Willenskraft, AttributeType.Charisma, AttributeType.Charisma, AttributeType.Wahrnehmung };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
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
                        new Skill(SkillType.Koerperbeherrschung),
                        new Skill(SkillType.Laufen),
                        new Skill(SkillType.Reiten),
                        new Skill(SkillType.Schwimmen),
                        new Skill(SkillType.Springen),
                        new Skill(SkillType.Tanzen)
                    };
                case SkillGroupType.Nahkampf:
                    return new List<Skill>
                    {
                        new Skill(SkillType.Dolche),
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
