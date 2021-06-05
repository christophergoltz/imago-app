using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class ArmorSet
    {
        public Dictionary<BodyPartType, Armor> ArmorParts { get; set; }

        public ArmorSet(Dictionary<BodyPartType, Armor> armorParts)
        {
            ArmorParts = armorParts;
        }
    }
}
