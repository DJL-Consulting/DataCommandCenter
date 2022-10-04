using System;
using System.Collections.Generic;

namespace DataCommandCenter.DAL.Models
{
    public partial class Header
    {
        public Header()
        {
            Properties = new HashSet<Property>();
            Servers = new HashSet<Server>();
            SqlColumns = new HashSet<SqlColumn>();
            SqlDatabases = new HashSet<SqlDatabase>();
            SqlObjects = new HashSet<SqlObject>();
        }

        public int Id { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<Server> Servers { get; set; }
        public virtual ICollection<SqlColumn> SqlColumns { get; set; }
        public virtual ICollection<SqlDatabase> SqlDatabases { get; set; }
        public virtual ICollection<SqlObject> SqlObjects { get; set; }
    }
}
