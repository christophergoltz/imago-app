using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class AttributeEntity : CreationExperienceBaseEntity
    {
        public AttributeType Type { get; set; }
        public int Corrosion { get; set; }
        public int ExperienceBySkillGroup { get; set; }
        public int BaseValue { get; set; }
    }
}
