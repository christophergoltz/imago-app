using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Application.Models.Template
{
    public class WeaponTemplateModel : WeightItemModel
    {
        public List<WeaponStance> WeaponStances { get; set; }
        public int DurabilityValue { get; set; }
    }
}
