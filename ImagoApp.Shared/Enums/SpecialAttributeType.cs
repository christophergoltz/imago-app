using ImagoApp.Shared.Attributes;

namespace ImagoApp.Shared.Enums
{
    public enum SpecialAttributeType
    {
        Unknown = 0,
        [Formula("(GE+GE+WA+WI)/4")]
        Initiative = 1
    }
}
