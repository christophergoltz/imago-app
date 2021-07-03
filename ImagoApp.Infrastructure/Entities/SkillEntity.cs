using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class SkillEntity : DependentBaseEntity
    {
        public SkillModelType Type { get; set; }
    }
}
