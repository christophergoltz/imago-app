﻿using ImagoApp.Shared.Enums;
using SQLite;

namespace ImagoApp.Infrastructure.Entities
{
    public class ArmorPartEntity : DurabilityItemEntity
    {
        public int PhysicalDefense { get; set; }
        public int EnergyDefense { get; set; }
        public ArmorPartType ArmorPartType { get; set; }
    }
}