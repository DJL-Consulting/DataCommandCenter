using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class Query
    {
        public string QueryName { get; set; } = null!;
        public string? QuerySql { get; set; }
    }
}
