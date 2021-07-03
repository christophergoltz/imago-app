using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Infrastructure.Entities;

namespace ImagoApp.Application.MappingProfiles
{
    public class CharacterMappingProfile : Profile
    {
        public CharacterMappingProfile()
        {
            //entity to model
            CreateMap<AttributeEntity, Models.Attribute>()
                .IncludeAllDerived();
            CreateMap<DerivedAttributeEntity, DerivedAttribute>()
                .IncludeAllDerived();
            CreateMap<SpecialAttributeEntity, SpecialAttribute>()
                .IncludeAllDerived();

            CreateMap<SkillGroupEntity, SkillGroupModel>()
                .IncludeAllDerived();
            CreateMap<SkillEntity, SkillModel>()
                .IncludeAllDerived();

            CreateMap<BloodCarrierEntity, BloodCarrierModel>();


            //model to entity
            CreateMap<Models.Attribute, AttributeEntity>();
            CreateMap<DerivedAttribute, DerivedAttributeEntity>();
            CreateMap<SpecialAttribute, SpecialAttributeEntity>();

            CreateMap<SkillGroupModel, SkillGroupEntity>();
            CreateMap<SkillModel, SkillEntity>();

            CreateMap<BloodCarrierModel, BloodCarrierEntity>();
        }
    }
}