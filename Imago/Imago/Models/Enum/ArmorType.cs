using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Enum
{
    public enum ArmorType
    {
        Unknown = 0,
        [DisplayText("Natürlich")]
        Natuerlich = 1,
        Komposit = 2,
        Chitin = 3,
        Platten = 4,
    }
}
