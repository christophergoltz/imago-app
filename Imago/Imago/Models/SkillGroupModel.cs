using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
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
