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

        public ProfessionType Type { get; set; }

        public Profession()
        {
            
        }

        public Profession(ProfessionType type, string formula)
        {
            Type = type;
            Formula = formula;
        }

        public string Formula
        {
            get => _formula;
            set => SetProperty(ref _formula, value);
        }

       
    }
}
