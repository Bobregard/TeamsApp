using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Data.Interfaces;

namespace TeamsApp.Data.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        private ITeamRepository _teamRepository;
        private IMatchRepository _matchRepository;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public ITeamRepository TeamRepository
        {
            get
            {
                if (_teamRepository is null)
                {
                    _teamRepository = new TeamRepository(_db);
                }
                return _teamRepository;
            }
        }
        public IMatchRepository MatchRepository
        {
            get
            {
                if (_matchRepository is null)
                {
                    _matchRepository = new MatchRepository(_db);
                }
                return _matchRepository;
            }
        }

        public Task SaveAsync() => _db.SaveChangesAsync();
    }
}
