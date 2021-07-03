﻿using System.Collections.Generic;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;

namespace ImagoApp.ViewModels
{
    public class OpenAttributeExperienceViewModel : Util.BindableBase
    {
        public SkillGroupModelType SourceType { get; set; }
        public List<Attribute> PossibleTargets { get; set; }
        
        public OpenAttributeExperienceViewModel(SkillGroupModelType sourceType, List<Attribute> possibleTargets)
        {
            SourceType = sourceType;
            PossibleTargets = possibleTargets;
        }
    }
}
