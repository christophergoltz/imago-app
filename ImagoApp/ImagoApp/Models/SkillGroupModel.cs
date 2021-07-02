using System.Collections.Generic;

namespace ImagoApp.Models
{
    public class SkillGroupModel : Base.DependentBase
    {
        public Enum.SkillGroupModelType Type { get; set; }

        public SkillGroupModel() { }

        public SkillGroupModel(Enum.SkillGroupModelType type)
        {
            Type = type;
        }

        public List<SkillModel> Skills { get; set; }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
