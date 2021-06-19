using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models.Enum
{
    public enum DerivedAttributeType
    {
        Unknown = 0,

        [Formula("WI/5")]
        Egoregenration = 1,

        [Formula("(ST/10)-5")]
        Schadensmod = 2, 

        [Formula("(KO+ST+ST)/10")]
        Traglast =3,

        [Formula("GE/10")]
        [DisplayText("Taktische Bewegung")]
        TaktischeBewegung = 6,

        [Formula("GE/5")]
        Sprintreichweite = 7,

        [Formula("Last/Traglast")]
        [DisplayText("Kampf")]

        BehinderungKampf = 8,
        [Formula("Last/Traglast")]
        [DisplayText("Abenteuer / Reise")]

        BehinderungAbenteuer = 9,
        [Formula("Last/Traglast")]
        [DisplayText("Gesamt")]
        BehinderungGesamt = 10,

        [DisplayText("Sprungreichweite Kampf")]
        [Formula("((GE+ST)-Last)/30")]
        SprungreichweiteKampf = 20,

        [Formula("((GE+ST)-Last)/80")]
        [DisplayText("Sprunghöhe Kampf")]
        SprunghoeheKampf= 21,

        [Formula("((GE+ST)-Last)/30")]
        [DisplayText("Sprungreichweite Abenteuer")]
        SprungreichweiteAbenteuer =22,

        [Formula("((GE+ST)-Last)/80")]
        [DisplayText("Sprunghöhe Abenteuer")]
        SprunghoeheAbenteuer = 23,

        [Formula("((GE+ST)-Last)/30")]
        [DisplayText("Sprungreichweite Gesamt")]
        SprungreichweiteGesamt =24,

        [Formula("((GE+ST)-Last)/80")]
        [DisplayText("Sprunghöhe Gesamt")]
        SprunghoeheGesamt = 25,
    }
}
