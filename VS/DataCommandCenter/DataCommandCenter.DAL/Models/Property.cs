using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class Property
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public string? Property1 { get; set; }
        public string? Value { get; set; }
        public DateTime? LastUpdateDatetime { get; set; }
        public string? LastUpdateUser { get; set; }

        public virtual Header Header { get; set; } = null!;
    }
}
