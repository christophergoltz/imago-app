namespace ImagoApp.Models.Enum
{
    public enum DerivedAttributeType
    {
        Unknown = 0,

        [Util.Formula("WI/5")]
        Egoregenration = 1,

        [Util.Formula("(ST/10)-5")]
        Schadensmod = 2, 

        [Util.Formula("(KO+ST+ST)/10")]
        Traglast =3,

        [Util.Formula("GE/10")]
        [Util.DisplayText("Taktische Bewegung")]
        TaktischeBewegung = 6,

        [Util.Formula("GE/5")]
        Sprintreichweite = 7,

        [Util.Formula("Last/Traglast")]
        [Util.DisplayText("Kampf")]

        BehinderungKampf = 8,
        [Util.Formula("Last/Traglast")]
        [Util.DisplayText("Abenteuer / Reise")]

        BehinderungAbenteuer = 9,
        [Util.Formula("Last/Traglast")]
        [Util.DisplayText("Gesamt")]
        BehinderungGesamt = 10,

        [Util.DisplayText("Sprungreichweite Kampf")]
        [Util.Formula("((GE+ST)-Last)/30")]
        SprungreichweiteKampf = 20,

        [Util.Formula("((GE+ST)-Last)/80")]
        [Util.DisplayText("Sprunghöhe Kampf")]
        SprunghoeheKampf= 21,

        [Util.Formula("((GE+ST)-Last)/30")]
        [Util.DisplayText("Sprungreichweite Abenteuer")]
        SprungreichweiteAbenteuer =22,

        [Util.Formula("((GE+ST)-Last)/80")]
        [Util.DisplayText("Sprunghöhe Abenteuer")]
        SprunghoeheAbenteuer = 23,

        [Util.Formula("((GE+ST)-Last)/30")]
        [Util.DisplayText("Sprungreichweite Gesamt")]
        SprungreichweiteGesamt =24,

        [Util.Formula("((GE+ST)-Last)/80")]
        [Util.DisplayText("Sprunghöhe Gesamt")]
        SprunghoeheGesamt = 25,
    }
}
