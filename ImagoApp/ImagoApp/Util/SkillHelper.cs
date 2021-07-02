namespace ImagoApp.Util
{
    public static class SkillHelper
    {
        public static void RecalculateFinalValue(this Models.SkillModel skillModel)
        {
            skillModel.FinalValue = skillModel.BaseValue + skillModel.IncreaseValue + skillModel.ModificationValue;
        }

        public static void RecalculateFinalValue(this Models.SkillGroupModel skillGroupModel)
        {
            skillGroupModel.FinalValue = skillGroupModel.BaseValue + skillGroupModel.IncreaseValue + skillGroupModel.ModificationValue;
        }

        public static void RecalculateFinalValue(this Models.Attribute attribute)
        {
            attribute.FinalValue = attribute.NaturalValue + attribute.IncreaseValue + attribute.ModificationValue - attribute.Corrosion;
        }
    }
}
