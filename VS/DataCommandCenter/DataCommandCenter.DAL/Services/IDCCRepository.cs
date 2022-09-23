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
        Task<IEnumerable<Server>> GetServers();
    }
}
