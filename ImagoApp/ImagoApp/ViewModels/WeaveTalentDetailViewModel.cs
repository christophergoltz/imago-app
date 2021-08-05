using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application.Models;

namespace ImagoApp.ViewModels
{
    public class WeaveTalentDetailViewModel
    {
        private readonly WeaveTalentModel _weaveTalent;
        private readonly SkillModel _skill;

        public WeaveTalentDetailViewModel(WeaveTalentModel weaveTalent, SkillModel skill)
        {
            _weaveTalent = weaveTalent;
            _skill = skill;
        }

        private void ParseFormula()
        {

        }
    }
}
