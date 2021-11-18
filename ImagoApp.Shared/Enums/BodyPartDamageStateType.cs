using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Attributes;

namespace ImagoApp.Shared.Enums
{
    public enum BodyPartDamageStateType
    {
        Normal = 0,
        [DisplayText("Ausgefallen")]
        Damaged = 1,
        [DisplayText("Zerstört")]
        Broken = 2
    }
}
