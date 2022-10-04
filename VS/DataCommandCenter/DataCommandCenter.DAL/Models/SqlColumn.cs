using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class SqlColumn
    {
        public int Id { get; set; }
        public int? ObjectId { get; set; }
        public string? ColumnName { get; set; }
        public string? DataType { get; set; }
        public int? MaxLength { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool? Nullable { get; set; }
        public bool? PrimaryKey { get; set; }
        public int? OrdinalPosition { get; set; }
        public string? Description { get; set; }
        public int? HeaderId { get; set; }

        public virtual Header? Header { get; set; }
        public virtual SqlObject? Object { get; set; }
    }
}
