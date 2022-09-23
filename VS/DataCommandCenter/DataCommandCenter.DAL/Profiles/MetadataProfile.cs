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
                //.ForAllMembers(opt => opt.Ignore())
                .ForMember(dest => dest.ServerType, opt => opt.MapFrom(src => src.ServerType.ServerType1));
        }
    }
}
