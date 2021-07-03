using System.Collections.Generic;
using SQLite;

namespace ImagoApp.Infrastructure.Entities
{
    public class WeaponEntity : DurabilityItemEntity
    {
        public List<WeaponStanceEntity> WeaponStances { get; set; }
    }
}
