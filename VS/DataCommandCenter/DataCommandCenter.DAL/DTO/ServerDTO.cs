using DataCommandCenter.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.DTO
{
    public class ServerDTO
    {

        public int Id { get; set; }
        public string? ServerName { get; set; }
        public string? ServerInstance { get; set; }
        public string? Version { get; set; }
        public bool? PullMetadata { get; set; }
        public string? ServerType { get; set; }

        //public virtual ServerType? ServerType { get; set; }
        //public virtual ICollection<SqlDatabase> SqlDatabases { get; set; }
    }
}
