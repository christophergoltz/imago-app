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
        [DisplayText("Taktische Bewegung")]
        TaktischeBewegung = 6,

        Sprintreichweite = 7,

        [DisplayText("Kampf")]
        BehinderungKampf = 8,
        [DisplayText("Abenteuer / Reise")]
        BehinderungAbenteuer = 9,
        [DisplayText("Gesamt")]
        BehinderungGesamt = 10,

        [DisplayText("Sprungreichweite Kampf")]
        SprungreichweiteKampf = 20,

        [DisplayText("Sprunghöhe Kampf")]
        SprunghoeheKampf= 21,

        [DisplayText("Sprungreichweite Abenteuer")]
        SprungreichweiteAbenteuer =22,

        [DisplayText("Sprunghöhe Abenteuer")]
        SprunghoeheAbenteuer = 23,

        [DisplayText("Sprungreichweite Gesamt")]
        SprungreichweiteGesamt =24,

        [DisplayText("Sprunghöhe Gesamt")]
        SprunghoeheGesamt = 25,
    }
}
