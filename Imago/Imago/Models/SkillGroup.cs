using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class SkillGroup : SkillBase
    {
        public SkillGroupType Type { get; set; }

        public SkillGroup() { }

        public SkillGroup(SkillGroupType type)
        {
            Type = type;
        }

        public List<Skill> Skills { get; set; }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
