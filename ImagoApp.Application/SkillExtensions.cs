using ImagoApp.Application.Models;

namespace ImagoApp.Application
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

        public static void RecalculateFinalValue(this AttributeModel attributeModel)
        {
            attributeModel.FinalValue = attributeModel.NaturalValue + attributeModel.IncreaseValue + attributeModel.ModificationValue - attributeModel.Corrosion;
        }
    }
}
