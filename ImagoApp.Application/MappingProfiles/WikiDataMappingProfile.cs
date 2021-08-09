using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Template;
using ImagoApp.Infrastructure.Entities;
using ImagoApp.Infrastructure.Entities.Template;

namespace ImagoApp.Application.MappingProfiles
{
    public class WikiDataMappingProfile : Profile
    {
        public WikiDataMappingProfile()
        {
            //entity to model
            CreateMap<WeaponTemplateEntity, WeaponTemplateModel>()
                .IncludeAllDerived();
            CreateMap<ArmorPartTemplateEntity, ArmorPartTemplateModel>()
                .IncludeAllDerived();

            CreateMap<MasteryEntity, MasteryModel>()
                .IncludeAllDerived();
            CreateMap<TalentEntity, TalentModel>()
                .IncludeAllDerived();
            CreateMap<WeaveTalentEntity, WeaveTalentModel>()
                .IncludeAllDerived();

            //model to entity
            CreateMap<WeaponTemplateModel, WeaponTemplateEntity>();
            CreateMap<ArmorPartTemplateModel, ArmorPartTemplateEntity>();
           
            CreateMap<MasteryModel, MasteryEntity>();
            CreateMap<TalentModel, TalentEntity>();
            CreateMap<WeaveTalentModel, WeaveTalentEntity>();
        }
    }
}