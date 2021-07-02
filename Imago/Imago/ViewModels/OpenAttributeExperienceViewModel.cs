using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.ViewModels
{
    public class OpenAttributeExperienceViewModel : BindableBase
    {
        public SkillGroupModelType SourceType { get; set; }
        public List<Attribute> PossibleTargets { get; set; }
        
        public OpenAttributeExperienceViewModel(SkillGroupModelType sourceType, List<Attribute> possibleTargets)
        {
            SourceType = sourceType;
            PossibleTargets = possibleTargets;
        }
    }
}
