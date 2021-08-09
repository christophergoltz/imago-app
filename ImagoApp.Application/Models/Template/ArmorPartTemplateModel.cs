using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models.Template
{
    public class ArmorPartTemplateModel : WeightItemModel
    {
        public int DurabilityValue { get; set; }
        public int PhysicalDefense { get; set; }
        public int EnergyDefense { get; set; }
        public ArmorPartType ArmorPartType { get; set; }
    }
}
