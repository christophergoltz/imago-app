using Imago.Util;

namespace Imago.Models.Enum
{
    public enum TableInfoType
    {
        [DisplayText("Rüstungen")]
        Armor = 0,
        [DisplayText("Nahkampfwaffen")]
        MeleeWeapons = 1,
        [DisplayText("Fernkampfwaffen")]
        RangedWeapons = 2,
        [DisplayText("Spezialwaffen")]
        SpecialWeapons = 3,
    }
}