using DataCommandCenter.DAL.DTO;
using DataCommandCenter.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static DataCommandCenter.DAL.Services.IDCCRepository;

namespace DataCommandCenter.DAL.Services
{
    public class DCCRepository : IDCCRepository
    {
        private readonly DataCommandCenterContext _context;

        public DCCRepository(DataCommandCenterContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Server>> GetServers()
        {
            return await _context.Servers.Include(s => s.ServerType).ToListAsync<Server>();
        }

        public async Task<IEnumerable<ObjectSearch>> SearchObjects(SearchObjectTypes options)
        {
            if (options == null)
                options = new SearchObjectTypes { Servers = true, Databases = true, Tables = true, Views = true, ProgrammableObjects = true, Columns = true };

            var searchQuery = options.QueryString.ToUpper();

            var collection = _context.ObjectSearches.Where(o => (
                                                                 (options.Servers == true && o.ObjectType == "Server")
                                                              || (options.Databases == true && o.ObjectType == "Database")
                                                              || (options.Tables == true && o.ObjectType == "SQL Table")
                                                              || (options.Views == true && o.ObjectType == "SQL VIEW")
                                                              || (options.Tables == true && (o.ObjectType == "SQL Table" || o.ObjectType == "SQL TYPE TABLE"))
                                                              || (options.ProgrammableObjects == true && (o.ObjectType == "SQL Stored Procedure" || o.ObjectType == "SQL Function" || o.ObjectType == "SQL  TABLE VALUED FUNCTION" || o.ObjectType == "SQL Trigger" || o.ObjectType == "SQL CLR SCALAR FUNCTION" || o.ObjectType == "SQL CLR STORED PROCEDURE" || o.ObjectType == "SQL CLR TABLE VALUED FUNCTION"))
                                                              || (options.Columns == true && o.ObjectType == "SQL Column")
                                                              || (options.Integrations == true && o.ObjectType == "Integration")
                                                              )
                                                              && o.SearchText.ToUpper().Contains(searchQuery)

                                                              ) as IQueryable<ObjectSearch>;

            //searchQuery = searchQuery.ToUpper();

            return await collection.ToListAsync<ObjectSearch>();

            //return await _context.ObjectSearches.Where(o => o.SearchText.Contains("")).ToListAsync<ObjectSearch>(); ;
        }

        public async Task<IEnumerable<Server>> GetServersForObject(ObjectSearch SelectedItem)
        {
            var objType = SelectedItem.ObjectType.ToUpper();

            switch (objType)
            {
                case "SERVER":
                    return await _context.Servers.Where(s => s.Id == SelectedItem.Id).Include(s => s.ServerType).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<Server>();
                case "DATABASE":
                    return await _context.Servers.Include(s => s.ServerType).Where(s => s.SqlDatabases.Any(d => d.Id == SelectedItem.Id)).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<Server>();
                case "SQL COLUMN":
                    return await _context.Servers.Include(s => s.ServerType).Where(s => s.SqlDatabases.Any(d => d.SqlObjects.Any(o => o.SqlColumns.Any(c => c.Id == SelectedItem.Id)))).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<Server>();
                default:  //Object - Table/view/programmable obj
                    return await _context.Servers.Include(s => s.ServerType).Where(s => s.SqlDatabases.Any(d => d.SqlObjects.Any(o => o.Id == SelectedItem.Id))).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<Server>();
            }
        }

        public async Task<IEnumerable<SqlDatabase>> GetDBsForObject(ObjectSearch SelectedItem)
        {
            var objType = SelectedItem.ObjectType.ToUpper();

            switch (objType)
            {
                case "SERVER":
                    var serverID = _context.SqlDatabases.Where(d => d.Id == SelectedItem.Id).FirstOrDefault().Id;
                    return await _context.SqlDatabases.Where(d => d.Server.Id == serverID).Include(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlDatabase>();  
                case "DATABASE":
                    return await _context.SqlDatabases.Where(d => d.Id == SelectedItem.Id).Include(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlDatabase>();  
                case "SQL COLUMN":
                    var col = _context.SqlColumns.Where(c => c.Id == SelectedItem.Id).Include(o => o.Object).FirstOrDefault();
                    return await _context.SqlDatabases.Where(d => d.Id == col.Object.DatabaseId).Include(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlDatabase>();
                default:  //Object - Table/view/programmable obj
                    var obj = _context.SqlObjects.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
                    return await _context.SqlDatabases.Where(d => d.Id == obj.DatabaseId).Include(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlDatabase>();
            }
        }

        public async Task<IEnumerable<SqlObject>> GetObjectsForObject(ObjectSearch SelectedItem)
        {
            var objType = SelectedItem.ObjectType.ToUpper();

            switch (objType)
            {
                case "SERVER":
                    return await _context.SqlObjects.Where(o => o.Database.Server.Id == -1).Include(o => o.Database).ThenInclude(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlObject>();
                //return await _context.SqlObjects.Where(o => o.Database.Server.Id == SelectedItem.Id).ToListAsync<SqlObject>();
                case "DATABASE":
                    return await _context.SqlObjects.Where(o => o.DatabaseId == SelectedItem.Id).Include(o => o.Database).ThenInclude(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlObject>();
                case "SQL COLUMN":
                    var col = _context.SqlColumns.Where(c => c.Id == SelectedItem.Id).Include(o => o.Object).FirstOrDefault();
                    return await _context.SqlObjects.Where(o => o.Id == col.ObjectId).Include(o => o.Database).ThenInclude(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlObject>();
                default:  //Object - Table/view/programmable obj
                    //this will return all objects in the database
                    var dbID = _context.SqlObjects.Where(o => o.Id == SelectedItem.Id).FirstOrDefault().DatabaseId;
                    return await _context.SqlObjects.Where(o => o.Database.Id == dbID).Include(o => o.Database).Include(o => o.Database.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlObject>();

                    //below will return only the selected object
                    //return await _context.SqlObjects.Where(o => o.Id == SelectedItem.Id).ToListAsync<SqlObject>();
            }
        }

        public async Task<IEnumerable<SqlColumn>> GetColumnsForObject(ObjectSearch SelectedItem)
        {
            var objType = SelectedItem.ObjectType.ToUpper();

            switch (objType)
            {
                case "SERVER":
                    return await _context.SqlColumns.Where(c => c.Object.Database.Server.Id == -1).Include(o => o.Object).ThenInclude(o => o.Database).ThenInclude(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlColumn>();
                //return await _context.SqlColumns.Where(c => c.Object.Database.Server.Id == SelectedItem.Id).ToListAsync<SqlColumn>();
                case "DATABASE":
                    return await _context.SqlColumns.Where(c => c.Object.Database.Id == -1).Include(o => o.Object).ThenInclude(o => o.Database).ThenInclude(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlColumn>();
                //return await _context.SqlColumns.Where(c => c.Object.Database.Id == SelectedItem.Id).ToListAsync<SqlColumn>();
                case "SQL COLUMN":
                    //this will return all columns in the object
                    var objID = _context.SqlColumns.Where(c => c.Id == SelectedItem.Id).Include(o => o.Object).ThenInclude(o => o.Database).ThenInclude(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).FirstOrDefault().ObjectId;
                    return await _context.SqlColumns.Where(c => c.Id == objID).ToListAsync<SqlColumn>();

                    //Below will return only the selected column
                    //return await _context.SqlColumns.Where(c => c.Id == SelectedItem.Id).ToListAsync<SqlColumn>();
                default:  //Object - Table/view/programmable obj
                    return await _context.SqlColumns.Where(c => c.Object.Id == SelectedItem.Id).Include(o => o.Object).ThenInclude(o => o.Database).ThenInclude(o => o.Server).Include(h => h.Header).ThenInclude(p => p.Properties).ToListAsync<SqlColumn>();
            }
        }

        public async Task<(IEnumerable<LineageFlow>, IEnumerable<SqlObject>)> GetLineageForObject(ObjectSearch SelectedItem, int levels = 4)
        {
            var objType = SelectedItem.ObjectType.ToUpper();

            if (objType == "INTEGRATION")
            {
                var flows = await _context.LineageFlows.Where(o => o.IntegrationFlowId != null).Include(l => l.IntegrationFlow).Include(i => i.IntegrationFlow.Integration).ToListAsync();

                var sourceIds = flows.Select(s => s.SourceObjectId);
                var destIds = flows.Select(d => d.DestinationObjectId);

                var sobjs = await _context.SqlObjects.Where(o => sourceIds.Contains(o.Id)).Include(d => d.Database).Include(d => d.Database.Server).Include(h => h.Header).Include(h => h.Header.Properties).ToListAsync();
                sobjs.ForEach(c => c.Level = 1);

                var dobjs = await _context.SqlObjects.Where(o => destIds.Contains(o.Id) && !sourceIds.Contains(o.Id)).Include(d => d.Database).ThenInclude(d => d.Server).Include(h => h.Header).Include(h => h.Header.Properties).ToListAsync();
                dobjs.ForEach(c => c.Level = 2);

                var objs = sobjs.Union(dobjs);

                return (flows, objs);
            }
            else
            {
                var thisObj = _context.SqlObjects.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();

                var startFlow = new LineageFlow { Id = -1, DestinationObject = thisObj, SourceObject = thisObj, Operation = "Root", DestinationObjectId = thisObj.Id, SourceObjectId = thisObj.Id };

                List<LineageFlow> l1;
                List<List<LineageFlow>> allFlows = new List<List<LineageFlow>>();
                List<SqlObject> allObjects = new List<SqlObject>();


                //Upstream
                l1 = new List<LineageFlow>();

                l1.Add(startFlow);

                var objsRoot = await _context.SqlObjects.Where(o => o.Id == thisObj.Id).Include(d => d.Database).ThenInclude(d => d.Server).Include(h => h.Header).Include(h => h.Header.Properties).ToListAsync();
                objsRoot[0].Level = levels + 1;
                allObjects = allObjects.Union(objsRoot).ToList();

                for (int x = levels; x > 0; x--)
                {
                    var ids = l1.Select(l => l.DestinationObjectId);

                    var l2 = await _context.LineageFlows.Where(f => ids.Contains(f.SourceObjectId)).Include(l => l.IntegrationFlow).Include(i => i.IntegrationFlow.Integration).ToListAsync<LineageFlow>();

                    var allIds = l2.Select(l => l.DestinationObjectId);

                    allFlows.Add(l2);

                    var objs = await _context.SqlObjects.Where(o => allIds.Contains(o.Id)).Include(d => d.Database).Include(d => d.Database.Server).Include(h => h.Header).Include(h => h.Header.Properties).ToListAsync();

                    objs.ForEach(c => c.Level = c.Level == null ? x : c.Level);

                    allObjects = allObjects.Union(objs).ToList(); // objs.Where(x => !allObjects.Any(o => o.Id == x.Id))).ToList();

                    l1 = l2;
                }

                //allObjects.Reverse();

                //Downstream
                l1 = new List<LineageFlow>();

                l1.Add(startFlow);

                for (int x = (levels + 2); x < (2 * levels + 1); x++)
                {
                    var ids = l1.Select(l => l.SourceObjectId);

                    var l2 = await _context.LineageFlows.Where(f => ids.Contains(f.DestinationObjectId)).Include(l => l.IntegrationFlow).Include(i => i.IntegrationFlow.Integration).ToListAsync<LineageFlow>();

                    allFlows.Add(l2);

                    var allIds = l2.Select(l => l.SourceObjectId);

                    var objs = await _context.SqlObjects.Where(o => allIds.Contains(o.Id)).Include(d => d.Database).Include(d => d.Database.Server).Include(h => h.Header).Include(h => h.Header.Properties).ToListAsync();

                    objs.ForEach(c => c.Level = c.Level == null ? x : c.Level);

                    allObjects = allObjects.Union(objs).ToList();// objs.Where(x => !allObjects.Any(o => o.Id == x.Id))).ToList();

                    l1 = l2;
                }

                var flows = allFlows.SelectMany(x => x).ToList();

                return (flows, allObjects);
            }
        }

        public DCCuser ValidateUserCredentials(string? userName, string? password)
        {
            var pwHash = this.GetHashString(password);

            var user = _context.Users.Where(u => u.UserName == userName && u.PasswordHash == pwHash).FirstOrDefault();

            if (user == null)
                return null;

            return new DCCuser(
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.UserType);

        }

        private string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }


    }
}
