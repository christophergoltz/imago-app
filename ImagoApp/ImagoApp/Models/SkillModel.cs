namespace ImagoApp.Models
{
    public class SkillModel : Base.DependentBase
    {
        public SkillModel()
        {
            
        }

        public SkillModelType Type { get; set; }

        public SkillModel(SkillModelType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
