using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models
{
    public class TalentModel : BindableBase
    {
        public TalentModel()
        {
            
        }

        public TalentModel(string name, Dictionary<SkillType, int> requirements,
            int? difficulty, bool activeUse, string phaseValueMod)
        {
            Name = name;
            Requirements = requirements;
            Difficulty = difficulty;
            ActiveUse = activeUse;
            PhaseValueMod = phaseValueMod;
        }

        public string Name  { get; set; }
        public Dictionary<SkillType, int> Requirements { get; set; }
        public int? Difficulty{ get; set; }
        public bool ActiveUse { get; set; }
        public string PhaseValueMod { get; set; }
    }
}
