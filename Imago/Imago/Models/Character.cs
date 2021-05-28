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
        public Race Race { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public string CreatedBy { get; set; }
        public string Owner { get; set; }

        public Version GameVersion { get; set; }

        public List<Attribute> Attributes { get; set; }

        public List<Skill> Bewegung { get; set; }
        public SkillGroup BewegungGroup { get; set; }

        public List<Skill> Nahkampf { get; set; }
        public SkillGroup NahkampfGroup { get; set; }

        public List<Skill> Heimlichkeit { get; set; }
        public SkillGroup HeimlichkeitGroup { get; set; }

        public List<Skill> Fernkampf { get; set; }
        public SkillGroup FernkampfGroup { get; set; }

        public List<Skill> Webkunst { get; set; }
        public SkillGroup WebkunstGroup { get; set; }

        public List<Skill> Wissenschaft { get; set; }
        public SkillGroup WissenschaftGroup { get; set; }

        public List<Skill> Handwerk { get; set; }
        public SkillGroup HandwerkGroup { get; set; }

        public List<Skill> Soziales { get; set; }
        public SkillGroup SozialesGroup { get; set; }
    }
}
