using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Models;

namespace TeamsApp.Data.Data.Interfaces
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<IEnumerable<Team>> GetAllTeamsRanked();
        Task<Team> GetTeamByName(string teamName);
        Task<IEnumerable<string>> GetAllTeamNames();
    }
}
