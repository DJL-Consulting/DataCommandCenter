using DataCommandCenter.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCommandCenter.DAL.Services
{
    public interface IDCCRepository
    {
        public class SearchObjectTypes
        {
            public string QueryString { get; set; }
            public bool Servers { get; set; }

            public bool Databases { get; set; }

            public bool Tables { get; set; }

            public bool Views { get; set; }

            public bool ProgrammableObjects { get; set; }

            public bool Columns { get; set; }
        }

        Task<IEnumerable<Server>> GetServers();

        Task<IEnumerable<ObjectSearch>> SearchObjects(SearchObjectTypes? options = null);
        Task<IEnumerable<ObjectSearch>> GetMetadataForObject(ObjectSearch SelectedItem);

    }
}
