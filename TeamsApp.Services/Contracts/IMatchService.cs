using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Models;
using TeamsApp.Services.DTOs;

namespace TeamsApp.Services.Contracts
{
    public interface IMatchService
    {
        Task<AddMatchDto> AddMatch(AddMatchDto matchDto);

        Task<MatchDto> GetMatchById(int id);

        Task<IEnumerable<MatchDto>> GetAllMatches();

        Task<AddMatchDto> EditMatch(int id, AddMatchDto match);

        Task DeleteMatch(int id);
    }
}
