using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Infrastructure.Entities
{
    public class BloodCarrierEntity : ItemBaseEntity
    {
        public int CurrentCapacity { get; set; }

        public int MaximumCapacity { get; set; }

        public int Regeneration { get; set; }
    }
}
