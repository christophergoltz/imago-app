using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class AttributeEntity : IncreasableBaseEntity
    {
        public AttributeType Type { get; set; }
        public int Corrosion { get; set; }
        public int NaturalValue { get; set; }
        public int ExperienceBySkillGroup { get; set; }
    }
}
