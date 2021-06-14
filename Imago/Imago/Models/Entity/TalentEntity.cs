using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;

namespace Imago.Models.Entity
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
