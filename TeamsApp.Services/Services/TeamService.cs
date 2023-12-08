using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Data.Interfaces;
using TeamsApp.Data.Models;
using TeamsApp.Services.Contracts;
using TeamsApp.Services.DTOs;

namespace TeamsApp.Services.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Team> AddTeam(TeamDto teamDto)
        {
            var teamNames = await _unitOfWork.TeamRepository.GetAllTeamNames();

            if (teamNames.Select(n => n.ToLower()).Contains(teamDto.Name.ToLower()))
            {
                throw new ArgumentException("Team with this name already exists");
            }
            if (teamDto.Goals < 0 || teamDto.Points < 0)
            {
                throw new ArgumentException("Points and Goals can't be negative numbers");
            }

            Team newTeam = new Team
            {
                Name = teamDto.Name,
                Points = teamDto.Points,
                Goals = teamDto.Goals
            };

            await _unitOfWork.TeamRepository.AddAsync(newTeam);
            await _unitOfWork.SaveAsync();

            return newTeam;
        }

        public async Task<Team> GetTeamById(int id)
        {
            var team = await _unitOfWork.TeamRepository.GetFirstOrDefaultAsync(n => n.Id == id);

            if (team == null)
            {
                throw new ArgumentException("Team not found");
            }

            return team;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsRanked()
        {
            var teams = await _unitOfWork.TeamRepository.GetAllTeamsRanked();

            if (!teams.Any())
            {
                throw new ArgumentException("No teams yet");
            }

            return teams;
        }

        public async Task<Team> EditTeam(int id, TeamDto team)
        {
            var existingTeam = await _unitOfWork.TeamRepository.GetFirstOrDefaultAsync(n => n.Id == id);
            var teamNames = await _unitOfWork.TeamRepository.GetAllTeamNames();

            if (existingTeam == null)
            {
                throw new ArgumentException("Team does not exist");
            }
            if (team.Name.ToLower() != existingTeam.Name.ToLower() && teamNames.Select(n => n.ToLower()).Contains(team.Name.ToLower()))
            {
                throw new ArgumentException("Team with this name already exists");
            }
            if (team.Goals < 0 || team.Points < 0)
            {
                throw new ArgumentException("Points and Goals can't be negative numbers");
            }

            existingTeam.Name = team.Name;
            existingTeam.Points = team.Points;
            existingTeam.Goals = team.Goals;
            
            await _unitOfWork.SaveAsync();

            return existingTeam;
        }

        public async Task DeleteTeam(int id)
        {
            var existingTeam = await _unitOfWork.TeamRepository.GetFirstOrDefaultAsync(n => n.Id == id);

            if (existingTeam == null)
            {
                throw new ArgumentException("Team does not exist");
            }

            await _unitOfWork.TeamRepository.DeleteAsync(existingTeam);
            await _unitOfWork.SaveAsync();
        }
    }
}
