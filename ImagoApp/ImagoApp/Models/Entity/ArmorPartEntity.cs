using Newtonsoft.Json;
using SQLite;

namespace ImagoApp.Models.Entity
{
    public class ArmorPartEntity : IJsonValueWrapper<ArmorPartModel>
    {
        public ArmorPartEntity()
        {
            
        }

        public string ValueAsJson { get; set; }
        
        [Ignore]
        public ArmorPartModel Value
        {
            get => JsonConvert.DeserializeObject<ArmorPartModel>(ValueAsJson);
            set => ValueAsJson = JsonConvert.SerializeObject(value);
        }
    }
}