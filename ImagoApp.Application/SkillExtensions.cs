using ImagoApp.Application.Models;

namespace ImagoApp.Util
{
    public static class SkillExtensions
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
