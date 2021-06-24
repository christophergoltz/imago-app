using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.ViewModels
{
    public class OpenAttributeExperienceViewModel : BindableBase
    {
        private Attribute _selectedAttribute;
        public List<Attribute> TargetAttributes { get; set; }
        public SkillGroupModelType Source { get; set; }

        public Attribute SelectedAttribute
        {
            get => _selectedAttribute;
            set => SetProperty(ref _selectedAttribute ,value);
        }

        public OpenAttributeExperienceViewModel(SkillGroupModelType source, List<Attribute> targetAttributes)
        {
            Source = source;
            TargetAttributes = targetAttributes;
        }
    }
}
