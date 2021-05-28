﻿
using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class SkillGroup : UpgradeableSkillBase
    {
        public List<AttributeType> SkillSource { get; set; }

        public SkillGroupType Type { get; set; }

        public SkillGroup()
        {
            
        }

        public SkillGroup(SkillGroupType type)
        {
            Type = type;
        }
    }
}
