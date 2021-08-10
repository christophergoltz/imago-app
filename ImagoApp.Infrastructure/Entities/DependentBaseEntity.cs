using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Infrastructure.Entities
{
    public class DependentBaseEntity : IncreasableBaseEntity
    {
        public int BaseValue { get; set; }
    }
}
