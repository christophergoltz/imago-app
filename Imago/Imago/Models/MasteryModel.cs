using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class MasteryModel : TalentBase
    {
        private Dictionary<SkillGroupType, int> _requirements;
        private string _phaseValueMod;

        public MasteryModel() : base()
        {

        }

        public MasteryModel(string name, string shortDescription, Dictionary<SkillGroupType, int> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription, activeUse, difficulty)
        {
            Requirements = requirements;
            PhaseValueMod = phaseValueMod;
        }


        public Dictionary<SkillGroupType, int> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements , value);
        }

        public string PhaseValueMod
        {
            get => _phaseValueMod;
            set => SetProperty(ref _phaseValueMod ,value);
        }
    }
}
