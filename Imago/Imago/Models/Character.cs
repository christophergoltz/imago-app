using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public string Height { get; set; }
        public string Weight { get; set; }
        public string EyeColor { get; set; }
        public string HairColor { get; set; }
        public string SkinColor { get; set; }
        public string Age { get; set; }
        public string DivineSoul { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public string CreatedBy { get; set; }
        public string Owner { get; set; }

        public Version GameVersion { get; set; }

        public List<Attribute> Attributes { get; set; }

        public Dictionary<SkillGroupType, SkillGroup> SkillGroups { get; set; }
        
        public ObservableCollection<SkillGroupType> OpenAttributeIncreases { get; set; }
    }
}
