using Newtonsoft.Json;
using SQLite;

namespace ImagoApp.Models.Entity
{
    public class WeaponEntity : IJsonValueWrapper<Weapon>
    {
        public WeaponEntity()
        {
            
        }

        public string ValueAsJson { get; set; }

        [Ignore]
        public Weapon Value
        {
            get => JsonConvert.DeserializeObject<Weapon>(ValueAsJson);
            set => ValueAsJson = JsonConvert.SerializeObject(value);
        }
    }
}
