using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Imago.Util
{
    public enum LogEntryType
    {
        Info = 0,
        Warning =1,
        Error = 2,
        Success = 3,
    }

    public class LogEntry
    {
        public LogEntry(LogEntryType type, string text)
        {
            Text = text;
            Timestamp = DateTime.Now;
            Type = type;
        }


        public string Text { get; set; }
        public DateTime Timestamp  { get; set; }
        public LogEntryType Type  { get; set; }
    }
}
