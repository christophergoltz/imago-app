using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class ArmorSet
    {
        public ArmorSet()
        {
            
        }

        public Dictionary<ArmorPartType, ArmorModel> ArmorParts { get; set; }

        public ArmorSet(Dictionary<ArmorPartType, ArmorModel> armorParts)
        {
            ArmorParts = armorParts;
        }
    }
}
