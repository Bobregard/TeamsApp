using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApp.Data.Data.Interfaces;
using TeamsApp.Data.Models;
using TeamsApp.Services.DTOs;
using TeamsApp.Services.Services;
using Match = TeamsApp.Data.Models.Match;

namespace TeamsApp.Tests
{
    [TestFixture]
    public class MatchServiceTests
    {
        private MatchService _matchService;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _matchService = new MatchService(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task AddMatch_ValidMatchDto_ReturnsMatchDto()
        {
            // Arrange
            var matchDto = new AddMatchDto
            {
                HomeTeamName = "HomeTeam",
                AwayTeamName = "AwayTeam",
                HomeTeamScore = 2,
                AwayTeamScore = 1
            };

            var homeTeam = new Team { Id = 1, Name = "HomeTeam" };
            var awayTeam = new Team { Id = 2, Name = "AwayTeam" };

            _unitOfWorkMock.Setup(u => u.TeamRepository.GetTeamByName(It.IsAny<string>())).ReturnsAsync((string name) =>
            {
                return name switch
                {
                    "HomeTeam" => homeTeam,
                    "AwayTeam" => awayTeam,
                    _ => null
                };
            });

            _unitOfWorkMock.Setup(u => u.MatchRepository.AddAsync(It.IsAny<Match>()));
            _unitOfWorkMock.Setup(u => u.SaveAsync());

            // Act
            var result = await _matchService.AddMatch(matchDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(matchDto, result);
        }

        [Test]
        public void AddMatch_InvalidTeams_ThrowsArgumentException()
        {
            // Arrange
            var matchDto = new AddMatchDto
            {
                HomeTeamName = "NonExistentTeam",
                AwayTeamName = "AwayTeam",
                HomeTeamScore = 2,
                AwayTeamScore = 1
            };

            _unitOfWorkMock.Setup(u => u.TeamRepository.GetTeamByName("NonExistentTeam")).ReturnsAsync((Team)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.AddMatch(matchDto));
        }

        [Test]
        public async Task GetMatchById_ExistingId_ReturnsMatchDto()
        {
            // Arrange
            var matchId = 1;
            var existingMatch = new Match { Id = matchId, HomeTeamId = 1, AwayTeamId = 2, HomeTeamScore = 2, AwayTeamScore = 1 };

            _unitOfWorkMock.Setup(u => u.MatchRepository.GetFirstOrDefaultAsync(m => m.Id == matchId)).ReturnsAsync(existingMatch);
            _unitOfWorkMock.Setup(u => u.TeamRepository.GetFirstOrDefaultAsync(t => t.Id == existingMatch.HomeTeamId)).ReturnsAsync(new Team { Id = 1, Name = "HomeTeam" });
            _unitOfWorkMock.Setup(u => u.TeamRepository.GetFirstOrDefaultAsync(t => t.Id == existingMatch.AwayTeamId)).ReturnsAsync(new Team { Id = 2, Name = "AwayTeam" });

            // Act
            var result = await _matchService.GetMatchById(matchId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(matchId, result.Id);
        }

        [Test]
        public void GetMatchById_NonExistingId_ThrowsArgumentException()
        {
            // Arrange
            var nonExistingMatchId = 999;

            _unitOfWorkMock.Setup(u => u.MatchRepository.GetFirstOrDefaultAsync(m => m.Id == nonExistingMatchId)).ReturnsAsync((Match)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.GetMatchById(nonExistingMatchId));
        }

        [Test]
        public void GetAllMatches_NoMatchesExist_ThrowsArgumentException()
        {
            // Arrange
            var emptyMatchesList = new List<Match>();

            _unitOfWorkMock.Setup(u => u.MatchRepository.GetAllMatches()).ReturnsAsync(emptyMatchesList);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.GetAllMatches());
        }

        [Test]
        public async Task EditMatch_ValidIdAndMatchDto_ReturnsEditedMatchDto()
        {
            // Arrange
            var matchId = 1;
            var editMatchDto = new AddMatchDto { HomeTeamName = "HomeTeam", AwayTeamName = "AwayTeam", HomeTeamScore = 3, AwayTeamScore = 2 };
            var existingMatch = new Match { Id = matchId, HomeTeamId = 1, AwayTeamId = 2, HomeTeamScore = 2, AwayTeamScore = 1 };

            var homeTeam = new Team { Id = 1, Name = "HomeTeam" };
            var awayTeam = new Team { Id = 2, Name = "AwayTeam" };

            _unitOfWorkMock.Setup(u => u.MatchRepository.GetFirstOrDefaultAsync(m => m.Id == matchId)).ReturnsAsync(existingMatch);
            _unitOfWorkMock.Setup(u => u.TeamRepository.GetTeamByName(It.IsAny<string>())).ReturnsAsync((string name) =>
            {
                return name switch
                {
                    "HomeTeam" => homeTeam,
                    "AwayTeam" => awayTeam,
                    _ => null
                };
            });

            _unitOfWorkMock.Setup(u => u.SaveAsync());

            // Act
            var result = await _matchService.EditMatch(matchId, editMatchDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(editMatchDto, result);
        }

        [Test]
        public void DeleteMatch_ExistingId_DeletesMatchAndSavesChanges()
        {
            // Arrange
            var matchId = 1;
            var existingMatch = new Match { Id = matchId, HomeTeamId = 1, AwayTeamId = 2, HomeTeamScore = 2, AwayTeamScore = 1 };

            _unitOfWorkMock.Setup(u => u.MatchRepository.GetFirstOrDefaultAsync(m => m.Id == matchId)).ReturnsAsync(existingMatch);
            _unitOfWorkMock.Setup(u => u.MatchRepository.DeleteAsync(existingMatch));
            _unitOfWorkMock.Setup(u => u.SaveAsync());

            // Act
            Assert.DoesNotThrowAsync(async () => await _matchService.DeleteMatch(matchId));
        }

        [Test]
        public void DeleteMatch_NonExistingId_ThrowsArgumentException()
        {
            // Arrange
            var nonExistingMatchId = 9999;

            _unitOfWorkMock.Setup(u => u.MatchRepository.GetFirstOrDefaultAsync(m => m.Id == nonExistingMatchId)).ReturnsAsync((Match)null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _matchService.DeleteMatch(nonExistingMatchId));
        }
    }
}
