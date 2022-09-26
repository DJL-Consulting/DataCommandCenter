using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class Server
    {
        public Server()
        {
            SqlDatabases = new HashSet<SqlDatabase>();
        }

        public int Id { get; set; }
        public int? ServerTypeId { get; set; }
        public string? ServerName { get; set; }
        public string? ServerInstance { get; set; }
        public string? Version { get; set; }
        public bool? PullMetadata { get; set; }
        public string? Description { get; set; }

        public virtual ServerType? ServerType { get; set; }
        public virtual ICollection<SqlDatabase> SqlDatabases { get; set; }
    }
}
