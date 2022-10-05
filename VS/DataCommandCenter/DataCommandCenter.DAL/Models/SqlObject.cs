using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataCommandCenter.DAL.Models
{
    public partial class SqlObject
    {
        public SqlObject()
        {
            LineageFlowDestinationObjects = new HashSet<LineageFlow>();
            LineageFlowSourceObjects = new HashSet<LineageFlow>();
            SqlColumns = new HashSet<SqlColumn>();
            SqlTableHistories = new HashSet<SqlTableHistory>();
        }

        public int Id { get; set; }
        public int? DatabaseId { get; set; }
        public string? SchemaName { get; set; }
        public string? ObjectName { get; set; }
        public string? ObjectType { get; set; }
        public int? Rows { get; set; }
        public double? SizeMb { get; set; }
        public string? ObjectDefinition { get; set; }
        public string? Description { get; set; }
        public int? HeaderId { get; set; }

        [NotMapped]
        public int? Level { get; set; }

        public virtual SqlDatabase? Database { get; set; }
        public virtual Header? Header { get; set; }
        public virtual ICollection<LineageFlow> LineageFlowDestinationObjects { get; set; }
        public virtual ICollection<LineageFlow> LineageFlowSourceObjects { get; set; }
        public virtual ICollection<SqlColumn> SqlColumns { get; set; }
        public virtual ICollection<SqlTableHistory> SqlTableHistories { get; set; }
    }
}
