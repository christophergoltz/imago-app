﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Models
{
    public class Character
    {
        private ObservableCollection<EquipableItem> _equippedItems;
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

        public int FreeSkillExperience { get; set; }

        public Version GameVersion { get; set; }

        public List<Attribute> Attributes { get; set; }
        public List<DerivedAttribute> DerivedAttributes { get; set; }
        public List<SpecialAttribute> SpecialAttributes { get; set; }

        public Dictionary<SkillGroupType, SkillGroup> SkillGroups { get; set; }

        public List<SkillGroupType> OpenAttributeIncreases { get; set; }

        public Dictionary<BodyPartType, BodyPart> BodyParts { get; set; }

        public WeaponType Weapon1 { get; set; }
        public WeaponType Weapon2 { get; set; }
        public WeaponType Weapon3 { get; set; }

        public ObservableCollection<EquipableItem> EquippedItems { get; set; }

        public List<DerivedAttribute> Handicap { get; set; }
    }
}
