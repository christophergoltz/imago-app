using System.Collections.Generic;

namespace ImagoApp.ViewModels
{
    public class OpenAttributeExperienceViewModel : Util.BindableBase
    {
        public Models.Enum.SkillGroupModelType SourceType { get; set; }
        public List<Models.Attribute> PossibleTargets { get; set; }
        
        public OpenAttributeExperienceViewModel(Models.Enum.SkillGroupModelType sourceType, List<Models.Attribute> possibleTargets)
        {
            SourceType = sourceType;
            PossibleTargets = possibleTargets;
        }
    }
}
