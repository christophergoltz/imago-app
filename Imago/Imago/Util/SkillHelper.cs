using System.Collections.Generic;
using System.Text;
using Imago.Models;
using Imago.Models.Base;

namespace Imago.Util
{
    public static class SkillHelper
    {
        public static void RecalculateFinalValue(this SkillModel skillModel)
        {
            skillModel.FinalValue = skillModel.BaseValue + skillModel.IncreaseValue + skillModel.ModificationValue;
        }

        public static void RecalculateFinalValue(this SkillGroupModel skillGroupModel)
        {
            skillGroupModel.FinalValue = skillGroupModel.BaseValue + skillGroupModel.IncreaseValue + skillGroupModel.ModificationValue;
        }

        public static void RecalculateFinalValue(this Attribute attribute)
        {
            attribute.FinalValue = attribute.NaturalValue + attribute.IncreaseValue + attribute.ModificationValue - attribute.Corrosion;
        }
    }
}
