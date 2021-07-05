using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models
{
    public class SkillRequirementModel
    {
        public SkillRequirementModel(SkillModelType type, int value)
        {
            Type = type;
            Value = value;
        }

        public SkillModelType Type { get; set; }
        public int Value { get; set; }
    }
}
