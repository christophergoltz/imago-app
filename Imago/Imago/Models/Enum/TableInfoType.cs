using Imago.Util;

namespace Imago.Models.Enum
{
    public enum TableInfoType
    {
        [DisplayText("Charaktere")] Character = 0,
        [DisplayText("Rüstungen")] Armor = 1,
        [DisplayText("Nahkampfwaffen")] MeleeWeapons = 2,
        [DisplayText("Fernkampfwaffen")] RangedWeapons = 3,
        [DisplayText("Spezialwaffen")] SpecialWeapons = 4,
        [DisplayText("Schilde")] Shields = 5,
        [DisplayText("Fertigkeiten")] Talents = 6,
        [DisplayText("Meisterschaften")] Masteries = 7,

        //todo Formen (Weben), Mächte, Gewalten, zb. Formen (Dolche)
    }
}