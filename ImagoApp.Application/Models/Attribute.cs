using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class Attribute : Base.IncreasableBase
    {
        //required for deserialization
        public Attribute()
        {
            
        }

        public Attribute(AttributeType type)
        {
            Type = type;
        }

        public AttributeType Type { get; set; } 

        private int _corrosion;
        private int _naturalValue;
        private int _experienceBySkillGroup;

        public int Corrosion
        {
            get => _corrosion;
            set => SetProperty(ref _corrosion , value);
        }
        
        public override string ToString()
        {
            return Type.ToString();
        }

        public int NaturalValue
        {
            get => _naturalValue;
            set => SetProperty(ref _naturalValue ,value);
        }

        public int ExperienceBySkillGroup
        {
            get => _experienceBySkillGroup;
            set => SetProperty(ref _experienceBySkillGroup, value);
        }
    }
}
