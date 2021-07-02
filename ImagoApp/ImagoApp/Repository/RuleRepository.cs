using System.Collections.Generic;

namespace ImagoApp.Repository
{
    public interface IRuleRepository
    {
        IEnumerable<Models.Enum.SkillGroupModelType> GetSkillGroupsByAttribute(Models.Enum.AttributeType type);
        List<Models.Enum.AttributeType> GetSkillGroupSources(Models.Enum.SkillGroupModelType modelType);
    }

    public class RuleRepository : IRuleRepository
    {
        private static readonly Dictionary<Models.Enum.SkillGroupModelType, List<Models.Enum.AttributeType>> SkillGroupAttributeLookUpDictionary =
            new Dictionary<Models.Enum.SkillGroupModelType, List<Models.Enum.AttributeType>>()
            {
                {Models.Enum.SkillGroupModelType.Bewegung, new List<Models.Enum.AttributeType> { Models.Enum.AttributeType.Staerke, Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Konstitution, Models.Enum.AttributeType.Konstitution }},
                {Models.Enum.SkillGroupModelType.Nahkampf, new List<Models.Enum.AttributeType> { Models.Enum.AttributeType.Staerke, Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Konstitution, Models.Enum.AttributeType.Wahrnehmung }},
                {Models.Enum.SkillGroupModelType.Heimlichkeit, new List<Models.Enum.AttributeType> { Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Intelligenz, Models.Enum.AttributeType.Willenskraft, Models.Enum.AttributeType.Wahrnehmung}},
                {Models.Enum.SkillGroupModelType.Fernkampf, new List<Models.Enum.AttributeType> { Models.Enum.AttributeType.Staerke, Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Wahrnehmung}},
                {Models.Enum.SkillGroupModelType.Webkunst, new List<Models.Enum.AttributeType> { Models.Enum.AttributeType.Willenskraft, Models.Enum.AttributeType.Willenskraft, Models.Enum.AttributeType.Charisma, Models.Enum.AttributeType.Charisma }},
                {Models.Enum.SkillGroupModelType.Wissenschaft, new List<Models.Enum.AttributeType> { Models.Enum.AttributeType.Intelligenz, Models.Enum.AttributeType.Intelligenz, Models.Enum.AttributeType.Intelligenz, Models.Enum.AttributeType.Wahrnehmung }},
                {Models.Enum.SkillGroupModelType.Handwerk, new List<Models.Enum.AttributeType> { Models.Enum.AttributeType.Geschicklichkeit, Models.Enum.AttributeType.Intelligenz, Models.Enum.AttributeType.Charisma, Models.Enum.AttributeType.Wahrnehmung }},
                {Models.Enum.SkillGroupModelType.Soziales, new List<Models.Enum.AttributeType> { Models.Enum.AttributeType.Willenskraft, Models.Enum.AttributeType.Charisma, Models.Enum.AttributeType.Charisma, Models.Enum.AttributeType.Wahrnehmung }}
            };

        public IEnumerable<Models.Enum.SkillGroupModelType> GetSkillGroupsByAttribute(Models.Enum.AttributeType type)
        {
            foreach (var kvp in SkillGroupAttributeLookUpDictionary)
            {
                if (kvp.Value.Contains(type))
                {
                    yield return kvp.Key;
                }
            }
        }
        
        public List<Models.Enum.AttributeType> GetSkillGroupSources(Models.Enum.SkillGroupModelType modelType)
        {
            return SkillGroupAttributeLookUpDictionary[modelType];
        }
    }
}
