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
            var newLine = "\n";

            CreateMap<Models.Server, DTO.ServerDTO>()
                .ForMember(dest => dest.ServerType, opt => opt.MapFrom(src => src.ServerType.ServerType1));
            CreateMap<Models.SqlDatabase, DTO.DatabaseDTO>();
            CreateMap<Models.SqlObject, DTO.ObjectDTO>();
            CreateMap<Models.SqlColumn, DTO.ColumnDTO>();
            CreateMap<Models.LineageFlow, DTO.LineageLink>()
                .ForMember(dest => dest.IntegrationInfo, opt => 
                    opt.MapFrom((src, dest) => 
                        { return (src.IntegrationFlow?.Integration?.IntegrationType + newLine + src.IntegrationFlow?.Integration?.Name); } 
                    ));
            CreateMap<Models.SqlObject, DTO.LineageNode>()
                .ForMember(dest => dest.Title, opt => 
                    opt.MapFrom((src, dest) =>
                        { return (src.Database?.Server?.ServerName + newLine + src.Database?.DatabaseName + newLine + (src.SchemaName.ToUpper() != "DBO" ? src.SchemaName + "." + src.ObjectName : src.ObjectName)); }
                    ));
        }
    }
}
