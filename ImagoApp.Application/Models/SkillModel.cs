using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class SkillModel : DependentBase
    {
        public SkillModel()
        {
            
        }

        public SkillModelType Type { get; set; }

        public SkillModel(SkillModelType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
