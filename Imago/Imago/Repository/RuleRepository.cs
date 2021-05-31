using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Repository
{
    public interface IRuleRepository
    {
        IEnumerable<SkillGroupType> GetSkillGroupsByAttribute(AttributeType type);
        List<AttributeType> GetSkillGroupSources(SkillGroupType type);
        List<AttributeType> GetProfessionGroupSources(ProfessionType type);
    }

    public class RuleRepository : IRuleRepository
    {
        public static readonly Dictionary<SkillGroupType, List<AttributeType>> SkillGroupAttributeLookUpDictionary =
            new Dictionary<SkillGroupType, List<AttributeType>>()
            {
                {SkillGroupType.Bewegung, new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Konstitution, AttributeType.Konstitution }},
                {SkillGroupType.Nahkampf, new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Konstitution, AttributeType.Wahrnehmung }},
                {SkillGroupType.Heimlichkeit, new List<AttributeType> { AttributeType.Geschicklichkeit, AttributeType.Intelligenz, AttributeType.Willenskraft, AttributeType.Wahrnehmung}},
                {SkillGroupType.Fernkampf, new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Geschicklichkeit, AttributeType.Wahrnehmung}},
                {SkillGroupType.Webkunst, new List<AttributeType> { AttributeType.Willenskraft, AttributeType.Willenskraft, AttributeType.Charisma, AttributeType.Charisma }},
                {SkillGroupType.Wissenschaft, new List<AttributeType> { AttributeType.Intelligenz, AttributeType.Intelligenz, AttributeType.Intelligenz, AttributeType.Wahrnehmung }},
                {SkillGroupType.Handwerk, new List<AttributeType> { AttributeType.Geschicklichkeit, AttributeType.Intelligenz, AttributeType.Charisma, AttributeType.Wahrnehmung }},
                {SkillGroupType.Soziales, new List<AttributeType> { AttributeType.Willenskraft, AttributeType.Charisma, AttributeType.Charisma, AttributeType.Wahrnehmung }}
            };

        private static readonly Dictionary<ProfessionType, List<AttributeType>> ProfessionGroupAttributeLookUpDictionary =
            new Dictionary<ProfessionType, List<AttributeType>>()
            {
                {ProfessionType.Initiative, new List<AttributeType> { AttributeType.Geschicklichkeit, AttributeType.Willenskraft, AttributeType.Wahrnehmung}},
                {ProfessionType.SchadensModifikator, new List<AttributeType> { AttributeType.Staerke}},
                {ProfessionType.EgoRegeneration, new List<AttributeType> { AttributeType.Willenskraft}},
                {ProfessionType.LastGrenze, new List<AttributeType> { AttributeType.Konstitution, AttributeType.Staerke}},
             };

        public IEnumerable<SkillGroupType> GetSkillGroupsByAttribute(AttributeType type)
        {
            foreach (var kvp in SkillGroupAttributeLookUpDictionary)
            {
                if (kvp.Value.Contains(type))
                {
                    yield return kvp.Key;
                }
            }
        }
        
        public List<AttributeType> GetSkillGroupSources(SkillGroupType type)
        {
            return SkillGroupAttributeLookUpDictionary[type];
        }

        public List<AttributeType> GetProfessionGroupSources(ProfessionType type)
        {
            return ProfessionGroupAttributeLookUpDictionary[type];
        }
    }
}
