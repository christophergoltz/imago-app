using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
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
