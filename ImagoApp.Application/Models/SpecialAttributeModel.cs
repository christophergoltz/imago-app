using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class SpecialAttributeModel : ModifiableBaseModel
    {
        private SpecialAttributeType _type;

        public SpecialAttributeModel()
        {
        }

        public SpecialAttributeModel(SpecialAttributeType type)
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