using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Shared
{
    public class CharacterPreview
    {
        public CharacterPreview(Guid guid, string name, string version, DateTime lastEdit, string filePath, long fileSize, DateTime lastBackup)
        {
            Guid = guid;
            Name = name;
            Version = version;
            LastEdit = lastEdit;
            FilePath = filePath;
            FileSize = fileSize;

            if (lastBackup == DateTime.MinValue)
                LastBackup = null;
            else
                LastBackup = lastBackup;
        }

        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime LastEdit { get; set; }
        public DateTime? LastBackup { get; set; }
        public int? LastBackupDaysAgo
        {
            get
            {
                if(LastBackup == null)
                    return null;

                return (DateTime.Today - LastBackup.Value).Days;
            }
        }

        public string FilePath { get; set; }
        public long FileSize { get; set; }
    }
}
