namespace ImagoApp.Models.Enum
{
    public enum SpecialAttributeType
    {
        Unknown = 0,
        [Util.Formula("(GE+GE+WA+WI)/4")]
        Initiative = 1
    }
}
