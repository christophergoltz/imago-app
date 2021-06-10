using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Imago.Models.Entity
{
    public class ArmorSetEntity : IJsonValueWrapper<ArmorSet>
    {
        public ArmorSetEntity()
        {
            
        }

        public string ValueAsJson { get; set; }
        
        [Ignore]
        public ArmorSet Value
        {
            get => JsonConvert.DeserializeObject<ArmorSet>(ValueAsJson);
            set => ValueAsJson = JsonConvert.SerializeObject(value);
        }
    }
}