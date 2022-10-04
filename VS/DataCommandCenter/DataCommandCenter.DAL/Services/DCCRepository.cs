using DataCommandCenter.DAL.DTO;
using DataCommandCenter.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    return await _context.Servers.Where(s => s.Id == SelectedItem.Id).Include(s => s.ServerType).ToListAsync<Server>();
                case "DATABASE":
                    return await _context.Servers.Include(s => s.ServerType).Where(s => s.SqlDatabases.Any(d => d.Id == SelectedItem.Id)).ToListAsync<Server>();
                case "SQL COLUMN":
                    return await _context.Servers.Include(s => s.ServerType).Where(s => s.SqlDatabases.Any(d => d.SqlObjects.Any(o => o.SqlColumns.Any(c => c.Id == SelectedItem.Id)))).ToListAsync<Server>();
                default:  //Object - Table/view/programmable obj
                    return await _context.Servers.Include(s => s.ServerType).Where(s => s.SqlDatabases.Any(d => d.SqlObjects.Any(o => o.Id == SelectedItem.Id))).ToListAsync<Server>();
            }
        }

        public async Task<IEnumerable<SqlDatabase>> GetDBsForObject(ObjectSearch SelectedItem)
        {
            var objType = SelectedItem.ObjectType.ToUpper();

            switch (objType)
            {
                case "SERVER":
                    var serverID = _context.SqlDatabases.Where(d => d.Id == SelectedItem.Id).FirstOrDefault().Id;
                    return await _context.SqlDatabases.Where(d => d.Server.Id == serverID).ToListAsync<SqlDatabase>();  
                case "DATABASE":
                    return await _context.SqlDatabases.Where(d => d.Id == SelectedItem.Id).ToListAsync<SqlDatabase>();  
                case "SQL COLUMN":
                    var col = _context.SqlColumns.Where(c => c.Id == SelectedItem.Id).Include(o => o.Object).FirstOrDefault();
                    return await _context.SqlDatabases.Where(d => d.Id == col.Object.DatabaseId).ToListAsync<SqlDatabase>();
                default:  //Object - Table/view/programmable obj
                    var obj = _context.SqlObjects.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
                    return await _context.SqlDatabases.Where(d => d.Id == obj.DatabaseId).ToListAsync<SqlDatabase>();
            }
        }

        public async Task<IEnumerable<SqlObject>> GetObjectsForObject(ObjectSearch SelectedItem)
        {
            var objType = SelectedItem.ObjectType.ToUpper();

            switch (objType)
            {
                case "SERVER":
                    return await _context.SqlObjects.Where(o => o.Database.Server.Id == -1).ToListAsync<SqlObject>();
                //return await _context.SqlObjects.Where(o => o.Database.Server.Id == SelectedItem.Id).ToListAsync<SqlObject>();
                case "DATABASE":
                    return await _context.SqlObjects.Where(o => o.DatabaseId == SelectedItem.Id).ToListAsync<SqlObject>();
                case "SQL COLUMN":
                    var col = _context.SqlColumns.Where(c => c.Id == SelectedItem.Id).Include(o => o.Object).FirstOrDefault();
                    return await _context.SqlObjects.Where(o => o.Id == col.ObjectId).ToListAsync<SqlObject>();
                default:  //Object - Table/view/programmable obj
                    //this will return all objects in the database
                    var dbID = _context.SqlObjects.Where(o => o.Id == SelectedItem.Id).FirstOrDefault().DatabaseId;
                    return await _context.SqlObjects.Where(o => o.Database.Id == dbID).ToListAsync<SqlObject>();

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
                    return await _context.SqlColumns.Where(c => c.Object.Database.Server.Id == -1).ToListAsync<SqlColumn>();
                //return await _context.SqlColumns.Where(c => c.Object.Database.Server.Id == SelectedItem.Id).ToListAsync<SqlColumn>();
                case "DATABASE":
                    return await _context.SqlColumns.Where(c => c.Object.Database.Id == -1).ToListAsync<SqlColumn>();
                //return await _context.SqlColumns.Where(c => c.Object.Database.Id == SelectedItem.Id).ToListAsync<SqlColumn>();
                case "SQL COLUMN":
                    //this will return all columns in the object
                    var objID = _context.SqlColumns.Where(c => c.Id == SelectedItem.Id).Include(o => o.Object).FirstOrDefault().ObjectId;
                    return await _context.SqlColumns.Where(c => c.Id == objID).ToListAsync<SqlColumn>();

                    //Below will return only the selected column
                    //return await _context.SqlColumns.Where(c => c.Id == SelectedItem.Id).ToListAsync<SqlColumn>();
                default:  //Object - Table/view/programmable obj
                    return await _context.SqlColumns.Where(c => c.Object.Id == SelectedItem.Id).ToListAsync<SqlColumn>();
            }
        }
    }
}
