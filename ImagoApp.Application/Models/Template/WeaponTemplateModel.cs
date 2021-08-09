using System.Collections.Generic;

namespace ImagoApp.Application.Models.Template
{
    public class WeaponTemplateModel : WeightItemModel
    {
        public List<WeaponStanceModel> WeaponStances { get; set; }
        public int DurabilityValue { get; set; }
    }
}
