using DataCommandCenter.DAL.DTO;
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
            public string SearchType { get; set; }
            public string QueryString { get; set; }
            public bool Servers { get; set; }

            public bool Databases { get; set; }

            public bool Tables { get; set; }

            public bool Views { get; set; }

            public bool ProgrammableObjects { get; set; }

            public bool Columns { get; set; }
            public bool Integrations { get; set; }
        }

        Task<IEnumerable<Server>> GetServers();

        Task<IEnumerable<ObjectSearch>> SearchObjects(SearchObjectTypes? options = null);

        Task<IEnumerable<Server>> GetAllServers();
        Task<IEnumerable<Server>> GetServersForObject(ObjectSearch SelectedItem);
        Task<IEnumerable<SqlDatabase>> GetAllDBs();
        Task<IEnumerable<SqlDatabase>> GetDBsForObject(ObjectSearch SelectedItem);
        Task<IEnumerable<SqlObject>> GetObjectsForObject(ObjectSearch SelectedItem);
        Task<IEnumerable<SqlColumn>> GetColumnsForObject(ObjectSearch SelectedItem);
        Task<(IEnumerable<LineageFlow>, IEnumerable<SqlObject>)> GetLineageForObject(ObjectSearch SelectedItem, int levels = 4);
        public DCCuser ValidateUserCredentials(string? userName, string? password);
    }
}
