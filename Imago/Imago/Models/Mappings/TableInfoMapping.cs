using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Imago.Database;
using Imago.Models.Enum;

namespace Imago.Models.Mappings
{
    public class TableInfoMapping : Profile
    {
        public TableInfoMapping()
        {
            CreateMap<TableInfoEntity, TableInfoModel>()
                .ForMember(model => model.Count, _ => _.Ignore())
                .ForMember(model => model.State, _ => _.Ignore())
                .AfterMap((entity, model) =>
                {
                    model.State = entity.TimeStamp == null ? TableInfoState.NoData : TableInfoState.Okay;
                });

            CreateMap<TableInfoModel, TableInfoEntity>();
        }
    }
}
