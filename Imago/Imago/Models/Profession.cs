using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Profession : SkillBase
    {
        public List<AttributeType> SkillSource { get; set; }
    }
}
