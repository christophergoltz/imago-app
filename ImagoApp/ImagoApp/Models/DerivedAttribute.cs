namespace ImagoApp.Models
{
    //eg. schadensmod, traglast
    public class DerivedAttribute : Base.CalculableBase
    {
        private Enum.DerivedAttributeType _type;

        public DerivedAttribute(Enum.DerivedAttributeType type)
        {
            Type = type;
        }

        public Enum.DerivedAttributeType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}