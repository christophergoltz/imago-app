using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Base;

namespace Imago.Util
{
    public static class SkillHelper
    {
        public static void RecalculateFinalValue(this UpgradeableSkillBase skillBase)
        {
            skillBase.FinalValue = skillBase.NaturalValue + skillBase.IncreaseValue + skillBase.ModificationValue;
        }

        public static void RecalculateFinalValue(this Attribute attribute)
        {
            attribute.FinalValue = attribute.NaturalValue + attribute.IncreaseValue + attribute.ModificationValue -
                                   attribute.Corrosion;
        }
    }
}
