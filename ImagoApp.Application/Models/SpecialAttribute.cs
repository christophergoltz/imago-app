using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class SpecialAttribute : Base.ModifiableBase
    {
        private SpecialAttributeType _type;

        public SpecialAttribute()
        {
        }

        public SpecialAttribute(SpecialAttributeType type)
        {
            Type = type;
        }

        public SpecialAttributeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}