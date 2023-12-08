using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Data.Interfaces;
using TeamsApp.Data.Data.Repositories;
using TeamsApp.Data.Models;
using TeamsApp.Services.Contracts;
using TeamsApp.Services.DTOs;

namespace TeamsApp.Services.Services
{
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddMatchDto> AddMatch(AddMatchDto matchDto)
        {
            var homeTeam = await _unitOfWork.TeamRepository.GetTeamByName(matchDto.HomeTeamName);
            var awayTeam = await _unitOfWork.TeamRepository.GetTeamByName(matchDto.AwayTeamName);

            if (awayTeam == null || homeTeam == null)
            {
                throw new ArgumentException("One or two of the teams do not exist");
            }

            if (matchDto.HomeTeamScore < 0 || matchDto.AwayTeamScore < 0)
            {
                throw new ArgumentException("Score can't be a negative number");
            }
            
            Match newMatch = new Match
            {
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,
                HomeTeamScore = matchDto.HomeTeamScore,
                AwayTeamScore = matchDto.AwayTeamScore
            };

            await _unitOfWork.MatchRepository.AddAsync(newMatch);

            UpdateTeamsGoalsAndPoints(newMatch, homeTeam, awayTeam);

            await _unitOfWork.SaveAsync();

            return matchDto;
        }

        public async Task<MatchDto> GetMatchById(int id)
        {
            var match = await _unitOfWork.MatchRepository.GetFirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                throw new ArgumentException("Match not found");
            }

            var matchResponse = new MatchDto
            {
                Id = id,
                HomeTeamName = _unitOfWork.TeamRepository.GetFirstOrDefaultAsync(t => t.Id == match.HomeTeamId).Result.Name,
                AwayTeamName = _unitOfWork.TeamRepository.GetFirstOrDefaultAsync(t => t.Id == match.AwayTeamId).Result.Name,
                HomeTeamScore = match.HomeTeamScore,
                AwayTeamScore = match.AwayTeamScore
            };

            return matchResponse;
        }

        public async Task<IEnumerable<MatchDto>> GetAllMatches()
        {
            var matches = await _unitOfWork.MatchRepository.GetAllMatches();

            if (!matches.Any())
            {
                throw new ArgumentException("No matches yet");
            }

            List<MatchDto> result = new List<MatchDto>();

            foreach (var match in matches)
            {
                result.Add(new MatchDto
                {
                    Id = match.Id,
                    HomeTeamName = _unitOfWork.TeamRepository.GetFirstOrDefaultAsync(t => t.Id == match.HomeTeamId).Result.Name,
                    AwayTeamName = _unitOfWork.TeamRepository.GetFirstOrDefaultAsync(t => t.Id == match.AwayTeamId).Result.Name,
                    HomeTeamScore = match.HomeTeamScore,
                    AwayTeamScore = match.AwayTeamScore
                });
            }

            return result;
        }

        public async Task<AddMatchDto> EditMatch(int id, AddMatchDto match)
        {
            var existingMatch = await _unitOfWork.MatchRepository.GetFirstOrDefaultAsync(m => m.Id == id);
            var homeTeam = await _unitOfWork.TeamRepository.GetTeamByName(match.HomeTeamName);
            var awayTeam = await _unitOfWork.TeamRepository.GetTeamByName(match.AwayTeamName);

            if (existingMatch == null)
            {
                throw new ArgumentException("Match does not exist");
            }
            if (awayTeam == null || homeTeam == null)
            {
                throw new ArgumentException("One or two of the teams do not exist");
            }
            if (match.HomeTeamScore < 0 || match.AwayTeamScore < 0)
            {
                throw new ArgumentException("Score can't be a negative number");
            }

            existingMatch.HomeTeamId = homeTeam.Id;
            existingMatch.AwayTeamId = awayTeam.Id;
            existingMatch.HomeTeamScore = match.HomeTeamScore;
            existingMatch.AwayTeamScore = match.AwayTeamScore;

            await _unitOfWork.SaveAsync();

            return match;
        }

        public async Task DeleteMatch(int id)
        {
            var existingMatch = await _unitOfWork.MatchRepository.GetFirstOrDefaultAsync(m => m.Id == id);

            if (existingMatch == null)
            {
                throw new ArgumentException("Match does not exist");
            }

            await _unitOfWork.MatchRepository.DeleteAsync(existingMatch);

            await _unitOfWork.SaveAsync();
        }

        private void UpdateTeamsGoalsAndPoints(Match match, Team homeTeam, Team awayTeam)
        {
            if (match.HomeTeamScore > match.AwayTeamScore)
            {
                homeTeam.Points += 3;
            }
            else if (match.HomeTeamScore < match.AwayTeamScore)
            {
                awayTeam.Points += 3;
            }
            else
            {
                homeTeam.Points++;
                awayTeam.Points++;
            }

            homeTeam.Goals += match.HomeTeamScore;
            awayTeam.Goals += match.AwayTeamScore;
        }
    }
}
