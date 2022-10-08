using DataCommandCenter.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.DTO
{
    public class DatabaseDTO
    {
        public int Id { get; set; }
        public int? ServerId { get; set; }
        public string? ServerName { get; set; }
        public string? DatabaseName { get; set; }
        public string Compatability { get; set; }
        public string? Recovery { get; set; }
        public string CreatedDatetime { get; set; }
        public string? Collation { get; set; }
        public string? Access { get; set; }
        public bool? ReadOnly { get; set; }
        public decimal? DataSizeMb { get; set; }
        public decimal? LogSizeMb { get; set; }
        public bool? PullMetadata { get; set; }
        public string? Description { get; set; }
        public ICollection<Property>? MetadataDictionary { get; set; }
    }
}
