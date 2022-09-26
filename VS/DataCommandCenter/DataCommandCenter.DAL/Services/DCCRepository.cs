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

        public async Task<IEnumerable<ObjectSearch>> SearchObjects(String searchQuery, SearchObjectTypes options)
        {
            if (options == null)
                options = new SearchObjectTypes();

            var colleciton = _context.ObjectSearches as IQueryable<ObjectSearch>;

            return await colleciton
                            .Where(o => o.SearchText.Contains(searchQuery)
                            ).ToListAsync<ObjectSearch>();
            
            //return await _context.ObjectSearches.Where(o => o.SearchText.Contains("")).ToListAsync<ObjectSearch>(); ;
        }

    }
}
