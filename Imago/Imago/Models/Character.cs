using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public RaceType RaceType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public string CreatedBy { get; set; }
        public string Owner { get; set; }

        public Version GameVersion { get; set; }

        public List<Attribute> Attributes { get; set; }

        public SkillGroup Bewegung { get; set; }
        public SkillGroup Nahkampf { get; set; }
        public SkillGroup Heimlichkeit { get; set; }
        public SkillGroup Fernkampf { get; set; }
        public SkillGroup Webkunst { get; set; }
        public SkillGroup Wissenschaft { get; set; }
        public SkillGroup Handwerk { get; set; }
        public SkillGroup Soziales { get; set; }

        public List<Profession> Professions { get; set; }
    }
}
