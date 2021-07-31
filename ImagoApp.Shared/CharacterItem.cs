using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Shared
{
    public class CharacterItem
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime LastEdit { get; set; }
    }
}
