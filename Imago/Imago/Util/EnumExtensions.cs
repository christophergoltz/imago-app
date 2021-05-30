using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imago.Util
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
