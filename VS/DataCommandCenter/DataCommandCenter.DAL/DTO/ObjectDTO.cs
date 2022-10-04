using DataCommandCenter.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.DTO
{
    public class ObjectDTO
    {
        public int Id { get; set; }
        public int? DatabaseId { get; set; }
        public string? SchemaName { get; set; }
        public string? ObjectName { get; set; }
        public string? ObjectType { get; set; }
        public int? Rows { get; set; }
        public double? SizeMb { get; set; }
        public string? ObjectDefinition { get; set; }
        public string? Description { get; set; }
        public ICollection<Property>? MetadataDictionary { get; set; }
    }
}
