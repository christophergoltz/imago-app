using Imago.Util;

namespace Imago.Models.Enum
{
    public enum TableInfoType
    {
        [DisplayText("Rüstungen")] Armor = 0,
        [DisplayText("Nahkampfwaffen")] MeleeWeapons = 1,
        [DisplayText("Fernkampfwaffen")] RangedWeapons = 2,
        [DisplayText("Spezialwaffen")] SpecialWeapons = 3,
        [DisplayText("Schilde")] Shields = 4,
        [DisplayText("Fertigkeiten")] Talents = 5,
        [DisplayText("Meisterschaften")] Masteries = 6,

        //todo Formen (Weben), Mächte, Gewalten, zb. Formen (Dolche)
    }
}