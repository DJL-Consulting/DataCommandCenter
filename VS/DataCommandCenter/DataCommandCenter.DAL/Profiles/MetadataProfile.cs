using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.Profiles
{
    public class MetadataProfile : Profile
    {
        public MetadataProfile()
        {
            CreateMap<Models.Server, DTO.ServerDTO>()
                .ForMember(dest => dest.ServerType, opt => opt.MapFrom(src => src.ServerType.ServerType1));
            CreateMap<Models.SqlDatabase, DTO.DatabaseDTO>();
            CreateMap<Models.SqlObject, DTO.ObjectDTO>();
            CreateMap<Models.SqlColumn, DTO.ColumnDTO>();
        }
    }
}
