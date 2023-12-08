using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsApp.Data.Data.Interfaces
{
    public interface IUnitOfWork
    {
        ITeamRepository TeamRepository { get; }
        IMatchRepository MatchRepository { get; }
        Task SaveAsync();
    }
}
