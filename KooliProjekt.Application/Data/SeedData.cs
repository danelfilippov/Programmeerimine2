using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;
        private static readonly Random _random = new Random();

        public SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Generate()
        {
            if (_dbContext.Users.Any())
            {
                return;
            }   

            var users = new List<User>();
            for (var i = 0; i < 5; i++)
            {
                var user = new User
                {
                    Name = "User " + (i + 1),
                    Email = "user" + (i + 1) + "@example.com",
                    Password = "password" + (i + 1),
                    Role = i % 2 == 0 ? "Admin" : "User",
                    Phone = "555000" + i
                };
                users.Add(user);
                _dbContext.Users.Add(user);
            }
            _dbContext.SaveChanges();

            var teams = new List<Team>();
            var teamNames = new[] { "Team A", "Team B", "Team C", "Team D", "Team E", "Team F" };
            var countries = new[] { "Estonia", "Latvia", "Lithuania", "Finland", "Sweden", "Norway" };
            for (var i = 0; i < teamNames.Length; i++)
            {
                var team = new Team
                {
                    Name = teamNames[i],
                    Country = countries[i]
                };
                teams.Add(team);
                _dbContext.Teams.Add(team);
            }
            _dbContext.SaveChanges();

            var tournaments = new List<Tournament>();
            for (var i = 0; i < 3; i++)
            {
                var tournament = new Tournament
                {
                    Name = "Tournament " + (i + 1),
                    Stage = i == 0 ? "Group" : i == 1 ? "Knockout" : "Final",
                    StartDate = DateTime.Now.AddDays(i * 10),
                    EndDate = DateTime.Now.AddDays(i * 10 + 7),
                    Description = "Test tournament " + (i + 1)
                };
                tournaments.Add(tournament);
                _dbContext.tournaments.Add(tournament);
            }
            _dbContext.SaveChanges();

            var matches = new List<Match>();
            var matchCounter = 0;
            foreach (var tournament in tournaments)
            {
                for (var i = 0; i < 4; i++)
                {
                    var match = new Match
                    {
                        TournamentId = tournament.Id,
                        HomeTeamId = teams[i].Id,
                        GuestTeamId = teams[(i + 1) % teams.Count].Id,
                        StartTime = tournament.StartDate.AddDays(i),
                        Status = i < 2 ? "Scheduled" : "Finished",
                        Stage = tournament.Stage,
                        Description = "Match " + (matchCounter + 1),
                        HomeScore = i < 2 ? 0 : _random.Next(0, 5),
                        GuestScore = i < 2 ? 0 : _random.Next(0, 5)
                    };
                    matches.Add(match);
                    _dbContext.Matchs.Add(match);
                    matchCounter++;
                }
            }
            _dbContext.SaveChanges();

            foreach (var user in users)
            {
                foreach (var tournament in tournaments)
                {
                    var leaderboard = new Leaderboard
                    {
                        UserId = user.Id,
                        TournamentId = tournament.Id,
                        TotalPoints = _random.Next(0, 100)
                    };
                    _dbContext.Leaderboards.Add(leaderboard);
                }
            }
            _dbContext.SaveChanges();

            foreach (var user in users)
            {
                foreach (var match in matches)
                {
                    var prediction = new Prediction
                    {
                        UserId = user.Id,
                        MatchId = match.Id,
                        HomeGoals = _random.Next(0, 5),
                        GuestGoals = _random.Next(0, 5),
                        Points = match.Status == "Finished" ? _random.Next(0, 5) : 0,
                        Status = match.Status == "Finished" ? "Evaluated" : "Pending"
                    };
                    _dbContext.Predictions.Add(prediction);
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
