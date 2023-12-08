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
    public class MatchRepository : Repository<Match>, IMatchRepository
    {
        public MatchRepository(AppDbContext db) : base(db)
        {
        }

        public async Task AddMatch(Match match)
        {
            await AddAsync(match);
        }

        public async Task DeleteMatch(Match match)
        {
            await DeleteAsync(match);
        }

        public async Task<IEnumerable<Match>> GetAllMatches()
        {
            var result = await GetAllAsync().Result.ToListAsync();
            return result;
        }

        public async Task<Match> GetMatchById(int matchId)
        {
            return await GetFirstOrDefaultAsync(b => b.Id == matchId);
        }
    }
}
