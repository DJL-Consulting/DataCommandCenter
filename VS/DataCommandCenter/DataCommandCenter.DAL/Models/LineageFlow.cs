using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class LineageFlow
    {
        public long Id { get; set; }
        public int? SourceObjectId { get; set; }
        public int? DestinationObjectId { get; set; }
        public int? IntegrationFlowId { get; set; }
        public string? Operation { get; set; }

        public virtual SqlObject? DestinationObject { get; set; }
        public virtual IntegrationFlow? IntegrationFlow { get; set; }
        public virtual SqlObject? SourceObject { get; set; }
    }
}
