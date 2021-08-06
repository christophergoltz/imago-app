using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class BodyPartEntity
    {
        public int MaxHitpoints { get; set; }
        public double CurrentHitpointsPercentage { get; set; }
        public List<ArmorPartEntity> Armor { get; set; }
        public BodyPartType Type { get; set; }
    }
}
