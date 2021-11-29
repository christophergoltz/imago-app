using ImagoApp.Application.Models.Base;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class SkillModel : CreationExperienceBaseModel
    {
        public SkillModel() : base(IncreaseType.Skill)
        {
            
        }

        public SkillModelType Type { get; set; }

        public SkillModel(SkillModelType type) : base(IncreaseType.Skill)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
