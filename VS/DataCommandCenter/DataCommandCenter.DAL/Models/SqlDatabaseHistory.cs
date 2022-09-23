using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class SqlDatabaseHistory
    {
        public int Id { get; set; }
        public int DatabaseId { get; set; }
        public DateTime CheckDatetime { get; set; }
        public decimal? DataSizeMb { get; set; }
        public decimal? LogSizeMb { get; set; }

        public virtual SqlDatabase Database { get; set; } = null!;
    }
}
