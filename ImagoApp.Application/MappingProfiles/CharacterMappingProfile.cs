using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;
using ImagoApp.Infrastructure.Entities;

namespace ImagoApp.Application.MappingProfiles
{
    public class CharacterMappingProfile : Profile
    {
        public CharacterMappingProfile()
        {
            //entity to model
            CreateMap<CharacterEntity, CharacterModel>()
                .IncludeAllDerived()
                .ForMember(model => model.LastBackup, opt => opt.Ignore())
                .AfterMap((entity, model) =>
                {
                    if (entity.LastBackup == DateTime.MinValue)
                        model.LastBackup = null;
                    else
                        model.LastBackup = entity.LastBackup;
                });
            CreateMap<AttributeEntity, AttributeModel>()
                .IncludeAllDerived();
            CreateMap<SpecialAttributeEntity, SpecialAttributeModel>()
                .IncludeAllDerived();

            CreateMap<SkillGroupEntity, SkillGroupModel>()
                .IncludeAllDerived();
            CreateMap<SkillEntity, SkillModel>()
                .IncludeAllDerived();

            CreateMap<BloodCarrierEntity, BloodCarrierModel>();

            CreateMap<IncreasableBaseEntity, IncreasableBaseModel>()
                .IncludeAllDerived()
                .ForMember(e => e.ExperienceForNextIncreasedRequiredCache, opt => opt.Ignore())
                .ForMember(e => e.IncreaseValueCache, opt => opt.Ignore())
                .ForMember(e => e.LeftoverExperienceCache, opt => opt.Ignore());

            CreateMap<CalculableBaseEntity, CalculableBaseModel>()
                .IncludeAllDerived()
                .ForMember(e => e.FinalValue, opt => opt.Ignore());
            CreateMap<DependentBaseEntity, DependentBaseModel>()
                .IncludeAllDerived()
                .ForMember(e => e.BaseValue, opt => opt.Ignore());
            CreateMap<CreationExperienceBaseEntity, CreationExperienceBaseModel>()
                .IncludeAllDerived();

            CreateMap<SkillRequirementEntity, SkillRequirementModel>();
            CreateMap<SkillGroupRequirementEntity, SkillGroupRequirementModel>();

            CreateMap<WeaponEntity, WeaponModel>()
                .IncludeAllDerived();
            CreateMap<WeaponStanceEntity, WeaponStanceModel>()
                .IncludeAllDerived();
            CreateMap<BodyPartEntity, BodyPartModel>()
                .IncludeAllDerived();
            CreateMap<ArmorPartEntity, ArmorPartModelModel>()
                .IncludeAllDerived();
            CreateMap<EquipableItemEntity, EquipableItemModel>()
                .IncludeAllDerived();
            CreateMap<WikiTabEntity, WikiTabModel>();

            //model to entity
            CreateMap<WeaponModel, WeaponEntity>();
            CreateMap<CharacterModel, CharacterEntity>()
                .ForMember(model => model.LastBackup, opt => opt.Ignore())
                .AfterMap((entity, model) =>
                {
                    if (entity.LastBackup == null)
                        model.LastBackup = DateTime.MinValue;
                    else
                        model.LastBackup = entity.LastBackup.Value;
                });
            CreateMap<AttributeModel, AttributeEntity>();
            CreateMap<SpecialAttributeModel, SpecialAttributeEntity>();

            CreateMap<SkillGroupModel, SkillGroupEntity>();
            CreateMap<SkillModel, SkillEntity>();

            CreateMap<BloodCarrierModel, BloodCarrierEntity>();

            CreateMap<IncreasableBaseModel, IncreasableBaseEntity>();
            CreateMap<CalculableBaseModel, CalculableBaseEntity>();
            CreateMap<DependentBaseModel, DependentBaseEntity>();
            CreateMap<CreationExperienceBaseModel, CreationExperienceBaseEntity>();

            CreateMap<SkillRequirementModel, SkillRequirementEntity>();
            CreateMap<SkillGroupRequirementModel, SkillGroupRequirementEntity>();

            CreateMap<BodyPartModel, BodyPartEntity>();
            CreateMap<WeaponStanceModel, WeaponStanceEntity>();
            CreateMap<ArmorPartModelModel, ArmorPartEntity>();
            CreateMap<EquipableItemModel, EquipableItemEntity>();
            CreateMap<WikiTabModel, WikiTabEntity>();
        }
    }
}