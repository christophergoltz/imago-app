using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Imago.Models.Entity
{
    public class ArmorSetEntity : IJsonValueWrapper<ArmorSet>
    {
        public ArmorSetEntity()
        {
            
        }

        public string JsonArmorSetValue { get; set; }

        public ArmorSet MapToModel()
        {
            return JsonConvert.DeserializeObject<ArmorSet>(JsonArmorSetValue);
        }
    }
}