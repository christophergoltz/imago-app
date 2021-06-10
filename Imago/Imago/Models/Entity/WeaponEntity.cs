using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Imago.Models.Entity
{
    public class WeaponEntity : IJsonValueWrapper<Weapon>
    {
        public string JsonWeaponValue { get; set; }
        public Weapon MapToModel()
        {
            return JsonConvert.DeserializeObject<Weapon>(JsonWeaponValue);
        }
    }
}
