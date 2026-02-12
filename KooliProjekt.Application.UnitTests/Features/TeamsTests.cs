using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Teams;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class TeamsTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetTeamsQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetTeamsQuery)null;
            var handler = new GetTeamsQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_request_id_is_null_or_negative(int id)
        {
            // Arrange
            var query = new GetTeamsQuery { Id = id };
            var handler = new GetTeamsQueryHandler(GetFaultyDbContext());

            var team = new Team { Title = "Test team" };
            await DbContext.Teams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_team()
        {
            // Arrange
            var query = new GetTeamsQuery { Id = 1 };
            var handler = new GetTeamsQueryHandler(DbContext);

            var team = new Team { Title = "Test team" };
            await DbContext.Teams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, ((Team)result.Value).Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_team_does_not_exist(int id)
        {
            // Arrange
            var query = new GetTeamsQuery { Id = id };
            var handler = new GetTeamsQueryHandler(DbContext);

            var team = new Team { Title = "Test team" };
            await DbContext.Teams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TeamsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (TeamsQuery)null;
            var handler = new TeamsQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        [InlineData(4, -10)]
        [InlineData(5, -5)]
        [InlineData(0, 0)]
        [InlineData(-5, -10)]
        public async Task List_should_return_null_when_page_or_page_size_is_zero_or_negative(int page, int pageSize)
        {
            // Arrange
            var query = new TeamsQuery { Page = page, PageSize = pageSize };
            var handler = new TeamsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_teams()
        {
            // Arrange
            var query = new TeamsQuery { Page = 1, PageSize = 5 };
            var handler = new TeamsQueryHandler(DbContext);

            foreach(var i in Enumerable.Range(1, 15))
            {
                var team = new Team { Title = $"Test team {i}" };
                await DbContext.Teams.AddAsync(team);
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_teams_doesnt_exist()
        {
            // Arrange
            var query = new TeamsQuery { Page = 1, PageSize = 5 };
            var handler = new TeamsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteTeamsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteTeamsCommand)null;
            var handler = new DeleteTeamsCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Delete_should_not_use_dbcontext_if_id_is_zero_or_less(int id)
        {
            // Arrange
            var query = new DeleteTeamsCommand { Id = id };
            var handler = new DeleteTeamsCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_team()
        {
            // Arrange
            var query = new DeleteTeamsCommand { Id = 1 };
            var handler = new DeleteTeamsCommandHandler(DbContext);

            var team = new Team { Title = "Test team" };
            await DbContext.Teams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Teams.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_team()
        {
            // Arrange
            var query = new DeleteTeamsCommand { Id = 1034 };
            var handler = new DeleteTeamsCommandHandler(DbContext);

            var team = new Team { Title = "Test team" };
            await DbContext.Teams.AddAsync(team);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveTeamsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveTeamsCommand)null;
            var handler = new SaveTeamsCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_when_id_is_negative()
        {
            // Arrange
            var request = new SaveTeamsCommand { Id = -10 };
            var handler = new SaveTeamsCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "Id");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_team()
        {
            // Arrange
            var request = new SaveTeamsCommand { Id = 0, Title = "New team" };
            var handler = new SaveTeamsCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedTeam = await DbContext.Teams.SingleOrDefaultAsync(t => t.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedTeam);
            Assert.Equal(1, savedTeam.Id);
        }

        [Fact]
        public async Task Save_should_save_existing_team()
        {
            // Arrange
            var teamToAdd = new Team { Id = 0, Title = "New team" };
            var request = new SaveTeamsCommand { Id = 1, Title = "Updated team" };
            var handler = new SaveTeamsCommandHandler(DbContext);

            await DbContext.Teams.AddAsync(teamToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedTeam = await DbContext.Teams.SingleOrDefaultAsync(t => t.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedTeam);
            Assert.Equal(request.Title, savedTeam.Title);
        }

        [Fact]
        public async Task Save_should_return_error_if_team_does_not_exist()
        {
            // Arrange
            var teamToAdd = new Team { Id = 0, Title = "New team" };
            var request = new SaveTeamsCommand { Id = 8, Title = "Updated team" };
            var handler = new SaveTeamsCommandHandler(DbContext);

            await DbContext.Teams.AddAsync(teamToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}
