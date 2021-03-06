using System.Collections.Generic;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Constants
{
    public static class RuleConstants
    {
        private static readonly Dictionary<SkillGroupModelType, List<AttributeType>> SkillGroupAttributeLookUpDictionary =
            new Dictionary<SkillGroupModelType, List<AttributeType>>()
            {
                {SkillGroupModelType.Bewegung, new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Konstitution, AttributeType.Konstitution }},
                {SkillGroupModelType.Nahkampf, new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Konstitution, AttributeType.Wahrnehmung }},
                {SkillGroupModelType.Heimlichkeit, new List<AttributeType> { AttributeType.Geschicklichkeit, AttributeType.Intelligenz, AttributeType.Willenskraft, AttributeType.Wahrnehmung}},
                {SkillGroupModelType.Fernkampf, new List<AttributeType> { AttributeType.Staerke, AttributeType.Geschicklichkeit, AttributeType.Geschicklichkeit, AttributeType.Wahrnehmung}},
                {SkillGroupModelType.Webkunst, new List<AttributeType> { AttributeType.Willenskraft, AttributeType.Willenskraft, AttributeType.Charisma, AttributeType.Charisma }},
                {SkillGroupModelType.Wissenschaft, new List<AttributeType> { AttributeType.Intelligenz, AttributeType.Intelligenz, AttributeType.Intelligenz, AttributeType.Wahrnehmung }},
                {SkillGroupModelType.Handwerk, new List<AttributeType> { AttributeType.Geschicklichkeit, AttributeType.Intelligenz, AttributeType.Charisma, AttributeType.Wahrnehmung }},
                {SkillGroupModelType.Soziales, new List<AttributeType> { AttributeType.Willenskraft, AttributeType.Charisma, AttributeType.Charisma, AttributeType.Wahrnehmung }}
            };

        public static IEnumerable<SkillGroupModelType> GetSkillGroupsByAttribute(AttributeType type)
        {
            foreach (var kvp in SkillGroupAttributeLookUpDictionary)
            {
                if (kvp.Value.Contains(type))
                {
                    yield return kvp.Key;
                }
            }
        }
        
        public static List<AttributeType> GetSkillGroupSources(SkillGroupModelType modelType)
        {
            return SkillGroupAttributeLookUpDictionary[modelType];
        }
    }
}
