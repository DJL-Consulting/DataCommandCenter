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
                                                              || (options.ProgrammableObjects == true && (o.ObjectType == "SQL Stored Procedure" || o.ObjectType == "SQL Function" || o.ObjectType == "SQL  TABLE VALUED FUNCTION" || o.ObjectType == "SQL Trigger" || o.ObjectType == "SQL CLR SCALAR FUNCTION" || o.ObjectType == "SQL CLR STORED PROCEDURE" || o.ObjectType == "SQL CLR TABLE VALUED FUNCTION" ))
                                                              || (options.Columns == true && o.ObjectType == "SQL Column")
                                                              )
                                                              && o.SearchText.ToUpper().Contains(searchQuery)

                                                              ) as IQueryable<ObjectSearch>;

            //searchQuery = searchQuery.ToUpper();

            return await collection.ToListAsync<ObjectSearch>();
            
            //return await _context.ObjectSearches.Where(o => o.SearchText.Contains("")).ToListAsync<ObjectSearch>(); ;
        }

        public async Task<IEnumerable<ObjectSearch>> GetMetadataForObject(ObjectSearch SelectedItem)
        {
            throw new NotImplementedException();
        }


    }
}
