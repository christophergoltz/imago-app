using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImagoApp.Application.Models;
using ImagoApp.Infrastructure.Entities;

namespace ImagoApp.Application.MappingProfiles
{
    public class WikiDataMappingProfile : Profile
    {
        public WikiDataMappingProfile()
        {
            //entity to model
            CreateMap<WeaponEntity, Weapon>()
                .IncludeAllDerived();
            CreateMap<WeaponStanceEntity, WeaponStance>()
                .IncludeAllDerived();

            CreateMap<ArmorPartEntity, ArmorPartModel>()
                .IncludeAllDerived();
            CreateMap<BodyPartEntity, BodyPart>()
                .IncludeAllDerived();

            CreateMap<MasteryEntity, MasteryModel>()
                .IncludeAllDerived();
            CreateMap<TalentEntity, TalentModel>()
                .IncludeAllDerived();


            //model to entity
            CreateMap<Weapon, WeaponEntity>();
            CreateMap<WeaponStance, WeaponStanceEntity>();

            CreateMap<ArmorPartModel, ArmorPartEntity>();
            CreateMap<BodyPart, BodyPartEntity>();

            CreateMap<MasteryModel, MasteryEntity>();
            CreateMap<TalentModel, TalentEntity>();
        }
    }
}