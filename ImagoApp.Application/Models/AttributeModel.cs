using ImagoApp.Application.Models.Base;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class AttributeModel : CreationExperienceBaseModel
    {
        //required for deserialization
        public AttributeModel() : base(IncreaseType.Attribute)
        {
            
        }

        public AttributeModel(AttributeType type) : base(IncreaseType.Attribute)
        {
            Type = type;
        }

        public AttributeType Type { get; set; } 

        private int _corrosion;
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
        
        public int ExperienceBySkillGroup
        {
            get => _experienceBySkillGroup;
            set => SetProperty(ref _experienceBySkillGroup, value);
        }
    }
}
