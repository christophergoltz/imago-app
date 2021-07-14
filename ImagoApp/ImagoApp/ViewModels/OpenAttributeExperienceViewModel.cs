using System.Collections.Generic;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;

namespace ImagoApp.ViewModels
{
    public class OpenAttributeExperienceViewModel : BindableBase
    {
        public SkillGroupModelType SourceType { get; set; }
        public List<AttributeModel> PossibleTargets { get; set; }
        
        public OpenAttributeExperienceViewModel(SkillGroupModelType sourceType, List<AttributeModel> possibleTargets)
        {
            SourceType = sourceType;
            PossibleTargets = possibleTargets;
        }
    }
}
