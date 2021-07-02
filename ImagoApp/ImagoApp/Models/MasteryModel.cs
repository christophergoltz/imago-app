using System.Collections.Generic;

namespace ImagoApp.Models
{
    public class MasteryModel : TalentBase
    {
        private Dictionary<Enum.SkillGroupModelType, int> _requirements;
        private Enum.SkillGroupModelType _targetSkill;


        public MasteryModel() : base()
        {

        }

        public MasteryModel(Enum.SkillGroupModelType targetSkill , string name, string shortDescription,string description, Dictionary<Enum.SkillGroupModelType, int> requirements,
            int? difficulty, bool activeUse, string phaseValueMod) : base(name, shortDescription, description, activeUse, difficulty, phaseValueMod)
        {
            TargetSkill = targetSkill;
            Requirements = requirements;
        }

        public Enum.SkillGroupModelType TargetSkill
        {
            get => _targetSkill;
            set => SetProperty(ref _targetSkill, value);
        }

        public Dictionary<Enum.SkillGroupModelType, int> Requirements
        {
            get => _requirements;
            set => SetProperty(ref _requirements , value);
        }

     
    }
}
