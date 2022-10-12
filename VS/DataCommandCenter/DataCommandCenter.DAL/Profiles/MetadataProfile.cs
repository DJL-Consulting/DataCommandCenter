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
        public string ConvertVersion(string version)
        {
            switch (version)
            {
                case "160":
                    return "SQL 2022";
                case "150":
                    return "SQL 2019";
                case "140":
                    return "SQL 2017";
                case "130":
                    return "SQL 2016";
                case "120":
                    return "SQL 2014";
                case "110":
                    return "SQL 2012";
                case "100":
                    return "SQL 2008";
            }
            return "SQL "+version;
        }

        public MetadataProfile()
        {
            var newLine = "\n";

            CreateMap<Models.Server, DTO.ServerDTO>()
                .ForMember(dest => dest.MetadataDictionary, opt => opt.MapFrom(src => src.Header.Properties))
                .ForMember(dest => dest.ServerType, opt => opt.MapFrom(src => src.ServerType.ServerType1))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => this.ConvertVersion(src.Version)));
            CreateMap<Models.SqlDatabase, DTO.DatabaseDTO>()
                .ForMember(dest => dest.MetadataDictionary, opt => opt.MapFrom(src => src.Header.Properties))
                .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Server.ServerName))
                .ForMember(dest => dest.Compatability, opt => opt.MapFrom(src => (src.Compatability == null ? "" : this.ConvertVersion(src.Compatability.ToString()))))
                .ForMember(dest => dest.CreatedDatetime, opt => opt.MapFrom(src => (src.CreatedDatetime == null ? "" : src.CreatedDatetime.ToString("MM/dd/yyyy h:mm tt"))));
            CreateMap<Models.SqlObject, DTO.ObjectDTO>()
                .ForMember(dest => dest.MetadataDictionary, opt => opt.MapFrom(src => src.Header.Properties))
                .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Database.Server.ServerName))
                .ForMember(dest => dest.DatabaseName, opt => opt.MapFrom(src => src.Database.DatabaseName));
            CreateMap<Models.SqlColumn, DTO.ColumnDTO>()
                .ForMember(dest => dest.MetadataDictionary, opt => opt.MapFrom(src => src.Header.Properties))
                .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Object.Database.Server.ServerName))
                .ForMember(dest => dest.DatabaseName, opt => opt.MapFrom(src => src.Object.Database.DatabaseName))
                .ForMember(dest => dest.ObjectName, opt => opt.MapFrom(src => src.Object.ObjectName));
            CreateMap<Models.Integration, DTO.LineageDTO>();
            CreateMap<Models.Property, DTO.PropertyDTO>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Property1));
            CreateMap<Models.IntegrationFlow, DTO.LineageDTO>();
            CreateMap<Models.LineageFlow, DTO.LineageLink>()
                .ForPath(dest => dest.Integration.Name, opt => opt.MapFrom(s => s.IntegrationFlow.Integration.Name))
                .ForPath(dest => dest.Integration.IntegrationType, opt => opt.MapFrom(s => s.IntegrationFlow.Integration.IntegrationType))
                .ForPath(dest => dest.Integration.Path, opt => opt.MapFrom(s => s.IntegrationFlow.Integration.Path))
                .ForPath(dest => dest.Integration.Description, opt => opt.MapFrom(s => s.IntegrationFlow.Integration.Description))
                .ForPath(dest => dest.Integration.Id, opt => opt.MapFrom(s => s.IntegrationFlow.Integration.Id))
                .ForPath(dest => dest.Integration.Created, opt => opt.MapFrom(s => s.IntegrationFlow.Integration.Created))
                .ForPath(dest => dest.Integration.LastModified, opt => opt.MapFrom(s => s.IntegrationFlow.Integration.LastModified))
                //.ForMember(dest => dest.Integration, opt => opt.MapFrom(s => s))
                //.ForMember(dest => dest.Integration, opt => opt.MapFrom(src => src.IntegrationFlow.Integration))
                //.ForMember(dest => dest.Integration, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.IntegrationInfo, opt => 
                    opt.MapFrom((src, dest) => 
                        { return (src.IntegrationFlow?.Integration?.IntegrationType + newLine + src.IntegrationFlow?.Integration?.Name); } 
                    ));
            CreateMap<Models.SqlObject, DTO.LineageNode>()
                .ForMember(dest => dest.MetadataDictionary, opt => opt.MapFrom(src => src.Header.Properties))
                .ForMember(dest => dest.Title, opt => 
                    opt.MapFrom((src, dest) =>
                        { return (dest.ObjectType.ToUpper() == "FILE" ? "File\n"+src.ObjectName : src.Database?.Server?.ServerName + newLine + src.Database?.DatabaseName + newLine + (src.SchemaName.ToUpper() != "DBO" ? src.SchemaName + "." + src.ObjectName : src.ObjectName)); }
                    ));
        }
    }
}
