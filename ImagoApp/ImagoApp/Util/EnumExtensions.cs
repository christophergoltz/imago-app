using System.Linq;

namespace ImagoApp.Util
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(System.Enum value)
            where TAttribute : System.Attribute
        {
            var type = value.GetType();
            var name = System.Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}
