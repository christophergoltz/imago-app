using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.MappingProfiles
{
    public class BaseMappingProfile : Profile
    {
        public BaseMappingProfile()
        {
            //entity to model
            CreateMap<DurabilityItemEntity, DurabilityItem>()
                .IncludeAllDerived();
            CreateMap<EquipableItemEntity, EquipableItem>()
                .IncludeAllDerived();
            CreateMap<ItemBaseEntity, ItemBase>()
                .IncludeAllDerived();
            CreateMap<TalentBaseEntity, TalentBase>()
                .IncludeAllDerived();
            CreateMap<CalculableBaseEntity, CalculableBase>()
                .IncludeAllDerived()
                .ForMember(member => member.FinalValue, options => options.Ignore());
            CreateMap<DependentBaseEntity, DependentBase>()
                .IncludeAllDerived()
                .ForMember(member => member.BaseValue, options => options.Ignore());
            CreateMap<IncreasableBaseEntity, IncreasableBase>()
                .IncludeAllDerived()
               .ForMember(member => member.ExperienceValue, options => options.Ignore())
                .ForMember(member => member.IncreaseValue, options => options.Ignore())
                .ForMember(member => member.ExperienceForNextIncreasedRequired, options => options.Ignore());
            CreateMap<RequirementModel<SkillGroupModelType>, RequirementEntity<SkillGroupModelType>>();
            CreateMap<RequirementModel<SkillModelType>, RequirementEntity<SkillModelType>>();

            //model to entity
            CreateMap<DurabilityItem, DurabilityItemEntity>();
            CreateMap<EquipableItem, EquipableItemEntity>();
            CreateMap<ItemBase, ItemBaseEntity>();
            CreateMap<TalentBase, TalentBaseEntity>(); 
            CreateMap<CalculableBase, CalculableBaseEntity>();
            CreateMap<IncreasableBase, IncreasableBaseEntity>();
            CreateMap<DependentBase, DependentBaseEntity>();
            CreateMap<RequirementEntity<SkillGroupModelType>, RequirementModel<SkillGroupModelType>>();
            CreateMap<RequirementEntity<SkillModelType>, RequirementModel<SkillModelType>>();
        }
    }
}