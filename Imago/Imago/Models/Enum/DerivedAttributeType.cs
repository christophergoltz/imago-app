using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Enum
{
    public enum DerivedAttributeType
    {
        Unknown = 0,

        Egoregenration = 1,

        Schadensmod = 2, 

        Traglast =3, 

        Sprungreichweite =4,

        [DisplayText("Sprunghöhe")]
        Sprunghoehe = 5,

        TaktischeBewegung = 6,

        Sprintreichweite = 7
    }
}
