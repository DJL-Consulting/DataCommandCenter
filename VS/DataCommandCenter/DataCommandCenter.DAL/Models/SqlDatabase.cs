using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class SqlDatabase
    {
        public SqlDatabase()
        {
            SqlDatabaseHistories = new HashSet<SqlDatabaseHistory>();
            SqlObjects = new HashSet<SqlObject>();
        }

        public int Id { get; set; }
        public int? ServerId { get; set; }
        public string? DatabaseName { get; set; }
        public byte Compatability { get; set; }
        public string? Recovery { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public string? Collation { get; set; }
        public string? Access { get; set; }
        public bool? ReadOnly { get; set; }
        public decimal? DataSizeMb { get; set; }
        public decimal? LogSizeMb { get; set; }
        public bool? PullMetadata { get; set; }

        public virtual Server? Server { get; set; }
        public virtual ICollection<SqlDatabaseHistory> SqlDatabaseHistories { get; set; }
        public virtual ICollection<SqlObject> SqlObjects { get; set; }
    }
}
