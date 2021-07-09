using System;
using System.Collections.Generic;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class SkillGroupModel : DependentBase
    {
        public SkillGroupModelType Type { get; set; }

        public SkillGroupModel() { }

        public SkillGroupModel(SkillGroupModelType type)
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
