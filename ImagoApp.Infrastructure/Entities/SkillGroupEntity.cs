using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Infrastructure.Entities
{
    public class SkillGroupEntity : DependentBaseEntity
    {
        public SkillGroupModelType Type { get; set; }

        public List<SkillEntity> Skills { get; set; }
    }
}
