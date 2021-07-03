using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class SpecialAttributeEntity : ModifiableBaseEntity
    {
        public SpecialAttributeType Type { get; set; }
    }
}
