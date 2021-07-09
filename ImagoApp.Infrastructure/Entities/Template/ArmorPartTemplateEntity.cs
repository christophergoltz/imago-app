
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities.Template
{
    public class ArmorPartTemplateEntity
    {
        public int PhysicalDefense { get; set; }
        public int EnergyDefense { get; set; }
        public ArmorPartType ArmorPartType { get; set; }
        public int DurabilityValue { get; set; }
        public string Name { get; set; }
        public int LoadValue { get; set; }
    }
}