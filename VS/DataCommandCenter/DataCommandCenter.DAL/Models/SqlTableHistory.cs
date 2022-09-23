using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class SqlTableHistory
    {
        public int Id { get; set; }
        public int? ObjectId { get; set; }
        public DateTime? CheckDatetime { get; set; }
        public int? Rows { get; set; }
        public double? SizeMb { get; set; }

        public virtual SqlObject? Object { get; set; }
    }
}
