using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Enum
{
    public enum WeaponStanceType
    {
        Unknown = 0,
        [DisplayText("Leichte Haltung")] Light,
        [DisplayText("Schwere Haltung")] Heavy
    }
}
