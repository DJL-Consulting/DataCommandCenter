using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class IntegrationFlow
    {
        public IntegrationFlow()
        {
            LineageFlows = new HashSet<LineageFlow>();
        }

        public int Id { get; set; }
        public int IntegrationId { get; set; }
        public string? SourceQuery { get; set; }
        public string? Description { get; set; }

        public virtual Integration Integration { get; set; } = null!;
        public virtual ICollection<LineageFlow> LineageFlows { get; set; }
    }
}
