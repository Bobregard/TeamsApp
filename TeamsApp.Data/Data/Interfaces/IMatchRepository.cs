using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Models;

namespace TeamsApp.Data.Data.Interfaces
{
    public interface IMatchRepository : IRepository<Match>
    {
        Task<IEnumerable<Match>> GetAllMatches();
    }
}
