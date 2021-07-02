namespace ImagoApp.Models.Enum
{
    public enum AttributeType
    {
        Unknown = 0,

        [Util.DisplayText("Stärke"), Util.Abbreviation("ST")] 
        Staerke = 1,

        [Util.Abbreviation("GE")] 
        Geschicklichkeit = 2,

        [Util.Abbreviation("KO")]
        Konstitution = 3,

        [Util.Abbreviation("IN")]
        Intelligenz = 4,

        [Util.Abbreviation("Wi")] 
        Willenskraft = 5,

        [Util.Abbreviation("CH")] 
        Charisma = 6,

        [Util.Abbreviation("WA")]
        Wahrnehmung = 7
    }
}