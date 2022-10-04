using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class LineageFlow
    {
        public long Id { get; set; }
        public long? SourceObjectId { get; set; }
        public long? DestinationObjectId { get; set; }
        public string? Operation { get; set; }
    }
}
