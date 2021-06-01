
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;

namespace Imago.Util
{
    public static class AttributesExtension
    {
        public static int GetFinalValueOfAttributeType(this List<Attribute> attributes, AttributeType type)
        {
            return attributes.First(_ => _.Type == type).FinalValue;
        }
    }
}
