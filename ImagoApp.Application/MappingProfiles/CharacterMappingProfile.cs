using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;
using ImagoApp.Application.Models.Template;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Entities.Template;

namespace ImagoApp.Application.MappingProfiles
{
    public class CharacterMappingProfile : Profile
    {
        public CharacterMappingProfile()
        {
            //entity to model
            CreateMap<CharacterEntity, Character>()
                .IncludeAllDerived();
            CreateMap<AttributeEntity, Models.Attribute>()
                .IncludeAllDerived();
            CreateMap<SpecialAttributeEntity, SpecialAttribute>()
                .IncludeAllDerived();

            CreateMap<SkillGroupEntity, SkillGroupModel>()
                .IncludeAllDerived();
            CreateMap<SkillEntity, SkillModel>()
                .IncludeAllDerived();

            CreateMap<BloodCarrierEntity, BloodCarrierModel>();

            CreateMap<IncreasableBaseEntity, IncreasableBase>()
                .IncludeAllDerived()
                .ForMember(e => e.ExperienceForNextIncreasedRequired, opt => opt.Ignore())
                .ForMember(e => e.IncreaseValue, opt => opt.Ignore())
                .ForMember(e => e.ExperienceValue, opt => opt.Ignore());
            CreateMap<CalculableBaseEntity, CalculableBase>()
                .IncludeAllDerived()
                .ForMember(e => e.FinalValue, opt => opt.Ignore());
            CreateMap<DependentBaseEntity, DependentBase>()
                .IncludeAllDerived()
                .ForMember(e => e.BaseValue, opt => opt.Ignore());

            CreateMap<SkillRequirementEntity, SkillRequirementModel>();
            CreateMap<SkillGroupRequirementEntity, SkillGroupRequirementModel>();

            CreateMap<WeaponEntity, Weapon>()
                .IncludeAllDerived();
            CreateMap<WeaponStanceEntity, WeaponStance>()
                .IncludeAllDerived();
            CreateMap<BodyPartEntity, BodyPart>()
                .IncludeAllDerived();
            CreateMap<ArmorPartEntity, ArmorPartModel>()
                .IncludeAllDerived();
            CreateMap<EquipableItemEntity, EquipableItem>()
                .IncludeAllDerived();
            CreateMap<WikiTabEntity, WikiTabModel>();

            //model to entity
            CreateMap<Weapon, WeaponEntity>();
            CreateMap<Character, CharacterEntity>();
            CreateMap<Models.Attribute, AttributeEntity>();
            CreateMap<SpecialAttribute, SpecialAttributeEntity>();

            CreateMap<SkillGroupModel, SkillGroupEntity>();
            CreateMap<SkillModel, SkillEntity>();

            CreateMap<BloodCarrierModel, BloodCarrierEntity>();

            CreateMap<IncreasableBase, IncreasableBaseEntity>();
            CreateMap<CalculableBase, CalculableBaseEntity>();
            CreateMap<DependentBase, DependentBaseEntity>();

            CreateMap<SkillRequirementModel, SkillRequirementEntity>();
            CreateMap<SkillGroupRequirementModel, SkillGroupRequirementEntity>();

            CreateMap<BodyPart, BodyPartEntity>();
            CreateMap<WeaponStance, WeaponStanceEntity>();
            CreateMap<ArmorPartModel, ArmorPartEntity>();
            CreateMap<EquipableItem, EquipableItemEntity>();
            CreateMap<WikiTabModel, WikiTabEntity>();
        }
    }
}