using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Infrastructure.Entities
{
    public class ModifiableBaseEntity : CalculableBaseEntity
    {
        public int ModificationValue { get; set; }
    }
}
