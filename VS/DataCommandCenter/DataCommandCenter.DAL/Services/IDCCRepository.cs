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
            bool SearchServers { get; set; } = true;
            bool SearchSQLDatabases { get; set; } = true;
            bool SearchSQLObjects { get; set; } = true;
            bool SearchSQLColumns { get; set; } = true;
        }

        Task<IEnumerable<Server>> GetServers();

        Task<IEnumerable<ObjectSearch>> SearchObjects(String searchQuery, SearchObjectTypes? options = null); 
    }
}
