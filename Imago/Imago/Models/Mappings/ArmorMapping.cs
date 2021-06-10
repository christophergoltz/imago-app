using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Imago.Models.Entity;
using Imago.Shared.Models;
using Newtonsoft.Json;

namespace Imago.Models.Mappings
{
    public class ArmorMapping : Profile
    {
        public ArmorMapping()
        {
            CreateMap<ArmorSetEntity, ArmorSet>()
                .ForMember(model => model.ArmorParts, _ => _.Ignore())
                .AfterMap((entity, model) =>
                {
                    model.ArmorParts = JsonConvert.DeserializeObject<Dictionary<ArmorPartType, ArmorModel>>(entity.JsonArmorSetValue);
                });

            CreateMap<ArmorSet, ArmorSetEntity>()
                .ForMember(entity => entity.JsonArmorSetValue, _ => _.Ignore())
                .AfterMap((entity, model) =>
                {
                    model.JsonArmorSetValue = JsonConvert.SerializeObject(entity.ArmorParts);
                });
        }
    }
}