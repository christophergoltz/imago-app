using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models
{
    public class TalentModel : TalentBase
    {
        private Dictionary<SkillType, int> _requirements;
        private string _phaseValueMod;

        public TalentModel() : base()
        {
            
        }

        public TalentModel(string name,string shortDescription, Dictionary<SkillType, int> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription, activeUse, difficulty)
        {
            Requirements = requirements;
            PhaseValueMod = phaseValueMod;
        }


        public Dictionary<SkillType, int> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements, value);
        }

        public string PhaseValueMod
        {
            get => _phaseValueMod;
            set => SetProperty(ref _phaseValueMod ,value);
        }
    }
}
