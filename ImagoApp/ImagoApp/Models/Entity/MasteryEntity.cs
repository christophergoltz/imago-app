using Newtonsoft.Json;
using SQLite;

namespace ImagoApp.Models.Entity
{
    public class MasteryEntity : IJsonValueWrapper<MasteryModel>
    {
        public MasteryEntity()
        {

        }

        public string ValueAsJson { get; set; }

        [Ignore]
        public MasteryModel Value
        {
            get => JsonConvert.DeserializeObject<MasteryModel>(ValueAsJson);
            set => ValueAsJson = JsonConvert.SerializeObject(value);
        }
    }
}
