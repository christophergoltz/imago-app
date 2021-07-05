using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class SkillGroupRequirementModel
    {
        public SkillGroupRequirementModel(SkillGroupModelType type, int value)
        {
            Type = type;
            Value = value;
        }

        public SkillGroupModelType Type { get; set; }
        public int Value { get; set; }
    }
}