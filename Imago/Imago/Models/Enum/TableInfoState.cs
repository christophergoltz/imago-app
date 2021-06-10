using Imago.Util;

namespace Imago.Models.Enum
{
    public enum TableInfoState
    {
        [DisplayText("Fehler")]
        Error = 0,
        [DisplayText("Wird geladen..")]
        Loading =1,
        [DisplayText("OK")]
        Okay = 2,
        [DisplayText("Keine Daten")]
        NoData = 3
    }
}