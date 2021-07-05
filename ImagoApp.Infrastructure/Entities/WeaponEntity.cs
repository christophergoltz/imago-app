using System.Collections.Generic;

namespace ImagoApp.Infrastructure.Entities
{
    public class WeaponEntity : DurabilityItemEntity
    {
        public List<WeaponStanceEntity> WeaponStances { get; set; }
    }
}
