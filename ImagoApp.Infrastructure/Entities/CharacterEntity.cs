using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImagoApp.Shared.Enums;
using SQLite;

namespace ImagoApp.Infrastructure.Entities
{
    public class CharacterEntity 
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Name { get; set; }
        public RaceType RaceType { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string EyeColor { get; set; }
        public string HairColor { get; set; }
        public string SkinColor { get; set; }
        public string Age { get; set; }
        public string DivineSoul { get; set; }
        public string CreatedBy { get; set; }
        public string Owner { get; set; }
        public string Note { get; set; }
        public int FreeSkillExperience { get; set; }

        public List<Attribute> Attributes { get; set; }
        public List<SkillGroupEntity> SkillGroups { get; set; }

        public ObservableCollection<SkillGroupModelType> OpenAttributeIncreases { get; set; }

        public List<BodyPartEntity> BodyParts { get; set; }

        public ObservableCollection<WeaponEntity> Weapons { get; set; }

        public ObservableCollection<EquipableItemEntity> EquippedItems { get; set; }

        public ObservableCollection<BloodCarrierEntity> BloodCarrier { get; set; }
    }
}