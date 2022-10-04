using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.DTO
{
    public class MetadataDTO
    {
        public IEnumerable<ServerDTO>? Servers { get; set; }
        public IEnumerable<DatabaseDTO>? Databases { get; set; }
        public IEnumerable<ObjectDTO>? Objects { get; set; }
        public IEnumerable<ColumnDTO>? Columns { get; set; }
    }
}
