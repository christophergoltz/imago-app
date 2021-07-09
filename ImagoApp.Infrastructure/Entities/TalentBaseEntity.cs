using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Infrastructure.Entities
{
    public class TalentBaseEntity
    {
        public string PhaseValueMod { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool ActiveUse { get; set; }
        public int? Difficulty { get; set; }
    }
}