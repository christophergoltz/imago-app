using Imago.Util;

namespace Imago.Models.Enum
{
    public enum TableInfoState
    {
        [DisplayText("Unbekannt")] Unknown = 0,
        [DisplayText("Fehler")] Error = 1,
        [DisplayText("Wird geladen..")] Loading = 2,
        [DisplayText("OK")] Okay = 3,
        [DisplayText("Keine Daten")] NoData = 4,
        [DisplayText("Keine Datenbank Datei")] NoDatebaseFile = 5
    }
}