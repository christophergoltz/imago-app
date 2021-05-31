using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Skill : UpgradeableSkillBase
    {
        public Skill()
        {
            
        }

        public SkillType Type { get; set; }

        public Skill(SkillType type)
        {
            Type = type;
        }
    }
}
