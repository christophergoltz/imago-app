using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Enum
{
    public enum WeaponType
    {
        Unknown,
        [DisplayText("Holzfälleraxt")]
        HolzfaellerAxt = 1,
        Dolch = 2,
        Blankbogen = 3,
        Schwert = 4,
    }
}
