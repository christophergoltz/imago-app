namespace ImagoApp.Models.Enum
{
    public enum TableInfoState
    {
        [Util.DisplayText("Unbekannt")] Unknown = 0,
        [Util.DisplayText("Fehler")] Error = 1,
        [Util.DisplayText("Wird geladen..")] Loading = 2,
        [Util.DisplayText("OK")] Okay = 3,
        [Util.DisplayText("Keine Daten")] NoData = 4,
        [Util.DisplayText("Keine Datenbank Datei")] NoDatebaseFile = 5
    }
}