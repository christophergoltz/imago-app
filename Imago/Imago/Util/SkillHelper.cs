using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Base;

namespace Imago.Util
{
    public static class SkillHelper
    {
        public static void RecalculateFinalValue(this Skill skill)
        {
            skill.FinalValue = skill.BaseValue + skill.IncreaseValue + skill.ModificationValue;
        }

        public static void RecalculateFinalValue(this SkillGroup skillGroup)
        {
            skillGroup.FinalValue = skillGroup.BaseValue + skillGroup.IncreaseValue + skillGroup.ModificationValue;
        }

        public static void RecalculateFinalValue(this Attribute attribute)
        {
            attribute.FinalValue = attribute.IncreaseValue + attribute.ModificationValue - attribute.Corrosion;
        }
    }
}
