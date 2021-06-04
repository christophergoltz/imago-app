using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.Models
{
    public class ChangeLogEntry
    {
        public ChangeLogEntry(string version, DateTime date, string title, string text )
        {
            Version = version;
            Text = text;
            Title = title;
            Date = date;
        }

        public string  Text { get; set; }
        public string  Title { get; set; }
        public string Version { get; set; }
        public DateTime Date { get; set; }
    }
}
