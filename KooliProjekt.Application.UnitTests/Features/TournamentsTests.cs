using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Tournaments;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class TournamentsTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetTournamentsQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetTournamentsQuery)null;
            var handler = new GetTournamentsQueryHandler(DbContext);

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
            var query = new GetTournamentsQuery { Id = id };
            var handler = new GetTournamentsQueryHandler(GetFaultyDbContext());

            var tournament = new Tournament { Title = "Test tournament" };
            await DbContext.tournaments.AddAsync(tournament);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_tournament()
        {
            // Arrange
            var query = new GetTournamentsQuery { Id = 1 };
            var handler = new GetTournamentsQueryHandler(DbContext);

            var tournament = new Tournament { Title = "Test tournament" };
            await DbContext.tournaments.AddAsync(tournament);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, ((Tournament)result.Value).Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_tournament_does_not_exist(int id)
        {
            // Arrange
            var query = new GetTournamentsQuery { Id = id };
            var handler = new GetTournamentsQueryHandler(DbContext);

            var tournament = new Tournament { Title = "Test tournament" };
            await DbContext.tournaments.AddAsync(tournament);
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
                new TournamentsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (TournamentsQuery)null;
            var handler = new TournamentsQueryHandler(DbContext);

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
            var query = new TournamentsQuery { Page = page, PageSize = pageSize };
            var handler = new TournamentsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_tournaments()
        {
            // Arrange
            var query = new TournamentsQuery { Page = 1, PageSize = 5 };
            var handler = new TournamentsQueryHandler(DbContext);

            foreach(var i in Enumerable.Range(1, 15))
            {
                var tournament = new Tournament { Title = $"Test tournament {i}" };
                await DbContext.tournaments.AddAsync(tournament);
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
        public async Task List_should_return_empty_result_if_tournaments_doesnt_exist()
        {
            // Arrange
            var query = new TournamentsQuery { Page = 1, PageSize = 5 };
            var handler = new TournamentsQueryHandler(DbContext);

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
                new DeleteTournamentsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteTournamentsCommand)null;
            var handler = new DeleteTournamentsCommandHandler(DbContext);

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
            var query = new DeleteTournamentsCommand { Id = id };
            var handler = new DeleteTournamentsCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_tournament()
        {
            // Arrange
            var query = new DeleteTournamentsCommand { Id = 1 };
            var handler = new DeleteTournamentsCommandHandler(DbContext);

            var tournament = new Tournament { Title = "Test tournament" };
            await DbContext.tournaments.AddAsync(tournament);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.tournaments.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_tournament()
        {
            // Arrange
            var query = new DeleteTournamentsCommand { Id = 1034 };
            var handler = new DeleteTournamentsCommandHandler(DbContext);

            var tournament = new Tournament { Title = "Test tournament" };
            await DbContext.tournaments.AddAsync(tournament);
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
                new SaveTournamentsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveTournamentsCommand)null;
            var handler = new SaveTournamentsCommandHandler(DbContext);

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
            var request = new SaveTournamentsCommand { Id = -10 };
            var handler = new SaveTournamentsCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "Id");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_tournament()
        {
            // Arrange
            var request = new SaveTournamentsCommand { Id = 0, Title = "New tournament" };
            var handler = new SaveTournamentsCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedTournament = await DbContext.tournaments.SingleOrDefaultAsync(t => t.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedTournament);
            Assert.Equal(1, savedTournament.Id);
        }

        [Fact]
        public async Task Save_should_save_existing_tournament()
        {
            // Arrange
            var tournamentToAdd = new Tournament { Id = 0, Title = "New tournament" };
            var request = new SaveTournamentsCommand { Id = 1, Title = "Updated tournament" };
            var handler = new SaveTournamentsCommandHandler(DbContext);

            await DbContext.tournaments.AddAsync(tournamentToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedTournament = await DbContext.tournaments.SingleOrDefaultAsync(t => t.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedTournament);
            Assert.Equal(request.Title, savedTournament.Title);
        }

        [Fact]
        public async Task Save_should_return_error_if_tournament_does_not_exist()
        {
            // Arrange
            var tournamentToAdd = new Tournament { Id = 0, Title = "New tournament" };
            var request = new SaveTournamentsCommand { Id = 8, Title = "Updated tournament" };
            var handler = new SaveTournamentsCommandHandler(DbContext);

            await DbContext.tournaments.AddAsync(tournamentToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}
