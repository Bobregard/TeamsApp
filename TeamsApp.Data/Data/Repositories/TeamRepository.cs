using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Data.Interfaces;
using TeamsApp.Data.Models;

namespace TeamsApp.Data.Data.Repositories
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Team>> GetAllTeamsRanked()
        {
            var result = await _db.Teams.OrderByDescending(team => team.Points).ThenByDescending(team => team.Goals).ToListAsync();
            return result;
        }

        public async Task<Team> GetTeamByName(string teamName)
        {
            return await GetFirstOrDefaultAsync(t => t.Name == teamName);
        }

        public async Task<IEnumerable<string>> GetAllTeamNames()
        {
            var result = await _db.Teams.Select(t => t.Name).ToListAsync();
            return result;
        }
    }
}
