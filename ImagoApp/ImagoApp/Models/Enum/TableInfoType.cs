namespace ImagoApp.Models.Enum
{
    public enum TableInfoType
    {
        [Util.DisplayText("Charaktere")] Character = 0,
        [Util.DisplayText("Rüstungen")] Armor = 1,
        [Util.DisplayText("Nahkampfwaffen")] MeleeWeapons = 2,
        [Util.DisplayText("Fernkampfwaffen")] RangedWeapons = 3,
        [Util.DisplayText("Spezialwaffen")] SpecialWeapons = 4,
        [Util.DisplayText("Schilde")] Shields = 5,
        [Util.DisplayText("Fertigkeiten")] Talents = 6,
        [Util.DisplayText("Meisterschaften")] Masteries = 7,

        //todo Formen (Weben), Mächte, Gewalten, zb. Formen (Dolche)
    }
}