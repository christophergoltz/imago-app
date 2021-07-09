using System.Collections.Generic;
namespace ImagoApp.Infrastructure.Entities.Template
{
    public class WeaponTemplateEntity
    {
        public List<WeaponStanceEntity> WeaponStances { get; set; }
        public int DurabilityValue { get; set; }
        public string Name { get; set; }
        public int LoadValue { get; set; }
    }
}
