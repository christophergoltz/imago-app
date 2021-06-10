using Imago.Shared.Util;

namespace Imago.Shared
{
    public enum ArmorModelType
    {
        Unknown = 0,
        [DisplayText("Natürlich")]
        Natuerlich = 1,
        Komposit = 2,
        Chitin = 3,
        Platten = 4,
        Ketten = 5,
        Stepp = 6,
        [DisplayText("Holz-/Knochen")]
        HolzKnochen = 7
    }
}