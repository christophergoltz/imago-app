using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Enum
{
    public enum SpecialAttributeType
    {
        Unknown = 0,
        [Formula("(GE+GE+WA+WI)/4")]
        Initiative = 1
    }
}
