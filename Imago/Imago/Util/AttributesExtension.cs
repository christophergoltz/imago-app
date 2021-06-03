﻿
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;

namespace Imago.Util
{
    public static class AttributesExtension
    {
        public static double GetFinalValueOfAttributeType(this List<Attribute> attributes, AttributeType type)
        {
            return attributes.First(_ => _.Type == type).FinalValue;
        }

        public static double GetFinalValueOfAttributeType(this List<DerivedAttribute> attributes, DerivedAttributeType type)
        {
            return attributes.First(_ => _.Type == type).FinalValue;
        }
    }
}
