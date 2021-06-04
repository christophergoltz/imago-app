using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;

namespace Imago.Repository
{
    public interface IChangeLogRepository
    {
        List<ChangeLogEntry> GetChangeLogEntries();
    }

    public class ChangeLogRepository : IChangeLogRepository
    {
        public List<ChangeLogEntry> GetChangeLogEntries()
        {
            return new List<ChangeLogEntry>()
            {
                new ChangeLogEntry("0.1", DateTime.Now, "Erstversion Imago.App",
                    "Hinzugefügt:\r\n\r\n- Spielerinfo\r\n\t- Attribute\r\n\t- Abgeleitete Attribute\r\n\t- Spezialattribute\r\n\t- Attributs-Steigerungen\r\n\t\r\n- Fertigkeiten\r\n\t- Fertigkeitssteigerungen\r\n\t\r\n- Status\r\n\t- Waffen\r\n\t- Rüstungen\r\n\t\r\n- Inventar\r\n\t- Last/Behinderung")
            }.OrderBy(entry => entry.Date).ToList();
        }
    }
}