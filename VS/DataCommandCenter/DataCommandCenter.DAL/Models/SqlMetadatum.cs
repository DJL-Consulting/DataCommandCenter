using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class SqlMetadatum
    {
        public string? ServerName { get; set; }
        public string ServerInstance { get; set; } = null!;
        public string? Version { get; set; }
        public string? ServerType { get; set; }
        public string? DatabaseName { get; set; }
        public string? Recovery { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public string? Collation { get; set; }
        public string? Access { get; set; }
        public bool? ReadOnly { get; set; }
        public decimal? DataSizeMb { get; set; }
        public decimal? LogSizeMb { get; set; }
        public string? SchemaName { get; set; }
        public string? ObjectName { get; set; }
        public string? ObjectType { get; set; }
        public int RowCount { get; set; }
        public double SizeMb { get; set; }
        public string ObjectDefinition { get; set; } = null!;
        public string? ColumnName { get; set; }
        public string? DataType { get; set; }
        public int? MaxLength { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool? Nullable { get; set; }
        public bool? PrimaryKey { get; set; }
        public int? OrdinalPosition { get; set; }
    }
}
