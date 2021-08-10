using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Shared
{
    public class CharacterPreview
    {
        public CharacterPreview(Guid guid, string name, string version, DateTime lastEdit, string filePath, long fileSize)
        {
            Guid = guid;
            Name = name;
            Version = version;
            LastEdit = lastEdit;
            FilePath = filePath;
            FileSize = fileSize;
        }

        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime LastEdit { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
    }
}
