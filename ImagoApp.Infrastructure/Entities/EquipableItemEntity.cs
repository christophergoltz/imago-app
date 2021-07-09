using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Infrastructure.Entities
{
    public class EquipableItemEntity : WeightItemEntity
    {
        public bool Fight { get; set; }
        public bool Adventure { get; set; }
    }
}
