namespace ImagoApp.Models
{
    //eg. Initiative
    public class SpecialAttribute : Base.ModifiableBase
    {
        private Enum.SpecialAttributeType _type;

        public SpecialAttribute()
        {
        }

        public SpecialAttribute(Enum.SpecialAttributeType type)
        {
            Type = type;
        }

        public Enum.SpecialAttributeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}