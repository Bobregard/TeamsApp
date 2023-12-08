using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Models;
using TeamsApp.Services.DTOs;

namespace TeamsApp.Services.Contracts
{
    public interface ITeamService
    {
        Task<Team> AddTeam(TeamDto team);

        Task<Team> GetTeamById(int id);

        Task<IEnumerable<Team>> GetAllTeamsRanked();

        Task<Team> EditTeam(int id, TeamDto team);

        Task DeleteTeam(int id);
    }
}
