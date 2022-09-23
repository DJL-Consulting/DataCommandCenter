using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class ServerType
    {
        public ServerType()
        {
            Servers = new HashSet<Server>();
        }

        public int Id { get; set; }
        public string? ServerType1 { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Server> Servers { get; set; }
    }
}
