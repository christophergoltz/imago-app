using Newtonsoft.Json;
using SQLite;

namespace ImagoApp.Models.Entity
{
    public class TalentEntity : IJsonValueWrapper<TalentModel>
    {
        public TalentEntity()
        {

        }

        public string ValueAsJson { get; set; }

        [Ignore]
        public TalentModel Value
        {
            get => JsonConvert.DeserializeObject<TalentModel>(ValueAsJson);
            set => ValueAsJson = JsonConvert.SerializeObject(value);
        }
    }
}
