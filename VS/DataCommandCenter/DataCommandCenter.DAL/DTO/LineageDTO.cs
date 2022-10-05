using DataCommandCenter.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.DTO
{
    public class LineageNode
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? SchemaName { get; set; }
        public string? ObjectName { get; set; }
        public string? ObjectType { get; set; }
        public int? Rows { get; set; }
        public double? SizeMb { get; set; }
        public string? ObjectDefinition { get; set; }
        public string? Description { get; set; }
        public IEnumerable<PropertyDTO> Properties { get; set; }
        public int? Level { get; set; }
    }

    public class LineageLink
    {
        public long Id { get; set; }
        public int? SourceObjectId { get; set; }
        public int? DestinationObjectId { get; set; }
        public int? IntegrationFlowId { get; set; }

        public string IntegrationInfo { get; set; }
        public string? Operation { get; set; }
    }
    public class LineageDTO
    {
        public IEnumerable<LineageNode> Nodes { get; set; }
        public IEnumerable<LineageLink> Flows { get; set; }
    }
}
