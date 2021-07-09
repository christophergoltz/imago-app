using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Infrastructure.Entities
{
    public class IncreasableBaseEntity : ModifiableBaseEntity
    {
        public int TotalExperience { get; set; }
    }
}
