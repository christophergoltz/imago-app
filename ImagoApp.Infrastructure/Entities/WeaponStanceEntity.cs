using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Infrastructure.Entities
{
    public class WeaponStanceEntity
    {
        public string Type { get; set; }
        public string PhaseValue { get; set; }
        public string DamageFormula { get; set; }
        public string ParryModifier { get; set; }
        public string Range { get; set; }
    }
}