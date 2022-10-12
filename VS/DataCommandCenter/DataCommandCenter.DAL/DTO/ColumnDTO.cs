using DataCommandCenter.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.DTO
{
    public class ColumnDTO
    {
        public int Id { get; set; }
        public int? ObjectId { get; set; }
        public string? ServerName { get; set; }
        public string? DatabaseName { get; set; }
        public string? ObjectName { get; set; }
        public string? ColumnName { get; set; }
        public string? DataType { get; set; }
        public int? MaxLength { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool? Nullable { get; set; }
        public bool? PrimaryKey { get; set; }
        public int? OrdinalPosition { get; set; }
        public string? Description { get; set; }
        public IEnumerable<PropertyDTO> MetadataDictionary { get; set; }
    }
}
