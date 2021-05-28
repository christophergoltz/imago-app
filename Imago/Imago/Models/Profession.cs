using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Base;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Profession : SkillBase
    {
        private string _formula;
        public List<AttributeType> SkillSource { get; set; }

        public ProfessionType Type { get; set; }

        public Profession()
        {
            
        }

        public string Formula
        {
            get => _formula;
            set => SetProperty(ref _formula, value);
        }

        public Profession(ProfessionType type, string formula, List<AttributeType> skillSource)
        {
            Type = type;
            SkillSource = skillSource;
            Formula = formula;
        }
    }
}
