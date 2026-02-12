using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Matchs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class MatchsTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetMatchsQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetMatchsQuery)null;
            var handler = new GetMatchsQueryHandler(DbContext);

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
            var query = new GetMatchsQuery { Id = id };
            var handler = new GetMatchsQueryHandler(GetFaultyDbContext());

            var match = new Match { Title = "Test match" };
            await DbContext.Matchs.AddAsync(match);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_match()
        {
            // Arrange
            var query = new GetMatchsQuery { Id = 1 };
            var handler = new GetMatchsQueryHandler(DbContext);

            var match = new Match { Title = "Test match" };
            await DbContext.Matchs.AddAsync(match);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, ((Match)result.Value).Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_match_does_not_exist(int id)
        {
            // Arrange
            var query = new GetMatchsQuery { Id = id };
            var handler = new GetMatchsQueryHandler(DbContext);

            var match = new Match { Title = "Test match" };
            await DbContext.Matchs.AddAsync(match);
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
                new MatchsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (MatchsQuery)null;
            var handler = new MatchsQueryHandler(DbContext);

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
            var query = new MatchsQuery { Page = page, PageSize = pageSize };
            var handler = new MatchsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_matches()
        {
            // Arrange
            var query = new MatchsQuery { Page = 1, PageSize = 5 };
            var handler = new MatchsQueryHandler(DbContext);

            foreach(var i in Enumerable.Range(1, 15))
            {
                var match = new Match { Title = $"Test match {i}" };
                await DbContext.Matchs.AddAsync(match);
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
        public async Task List_should_return_empty_result_if_matches_doesnt_exist()
        {
            // Arrange
            var query = new MatchsQuery { Page = 1, PageSize = 5 };
            var handler = new MatchsQueryHandler(DbContext);

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
                new DeleteMatchsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteMatchsCommand)null;
            var handler = new DeleteMatchsCommandHandler(DbContext);

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
            var query = new DeleteMatchsCommand { Id = id };
            var handler = new DeleteMatchsCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_match()
        {
            // Arrange
            var query = new DeleteMatchsCommand { Id = 1 };
            var handler = new DeleteMatchsCommandHandler(DbContext);

            var match = new Match { Title = "Test match" };
            await DbContext.Matchs.AddAsync(match);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Matchs.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_match()
        {
            // Arrange
            var query = new DeleteMatchsCommand { Id = 1034 };
            var handler = new DeleteMatchsCommandHandler(DbContext);

            var match = new Match { Title = "Test match" };
            await DbContext.Matchs.AddAsync(match);
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
                new SaveMatchsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveMatchsCommand)null;
            var handler = new SaveMatchsCommandHandler(DbContext);

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
            var request = new SaveMatchsCommand { Id = -10 };
            var handler = new SaveMatchsCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "Id");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_match()
        {
            // Arrange
            var request = new SaveMatchsCommand { Id = 0, Title = "New match" };
            var handler = new SaveMatchsCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedMatch = await DbContext.Matchs.SingleOrDefaultAsync(m => m.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedMatch);
            Assert.Equal(1, savedMatch.Id);
        }

        [Fact]
        public async Task Save_should_save_existing_match()
        {
            // Arrange
            var matchToAdd = new Match { Id = 0, Title = "New match" };
            var request = new SaveMatchsCommand { Id = 1, Title = "Updated match" };
            var handler = new SaveMatchsCommandHandler(DbContext);

            await DbContext.Matchs.AddAsync(matchToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedMatch = await DbContext.Matchs.SingleOrDefaultAsync(m => m.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedMatch);
            Assert.Equal(request.Title, savedMatch.Title);
        }

        [Fact]
        public async Task Save_should_return_error_if_match_does_not_exist()
        {
            // Arrange
            var matchToAdd = new Match { Id = 0, Title = "New match" };
            var request = new SaveMatchsCommand { Id = 8, Title = "Updated match" };
            var handler = new SaveMatchsCommandHandler(DbContext);

            await DbContext.Matchs.AddAsync(matchToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}
