using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    //todo obsolete?
    public class DerivedAttributeEntity : CalculableBaseEntity
    {
        public DerivedAttributeType Type { get; set; }
    }
}
