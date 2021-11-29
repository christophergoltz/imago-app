using System.Collections.Generic;
using ImagoApp.Application.Models.Base;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class SkillGroupModel : DependentBaseModel
    {
        public SkillGroupModelType Type { get; set; }

        public SkillGroupModel() : base(IncreaseType.SkillGroup)
        {

        }

        public SkillGroupModel(SkillGroupModelType type) : base(IncreaseType.SkillGroup)
        {
            Type = type;
        }

        public List<SkillModel> Skills { get; set; }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
