using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Leaderboards;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class LeaderboardsTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetLeaderboardsQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetLeaderboardsQuery)null;
            var handler = new GetLeaderboardsQueryHandler(DbContext);

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
            var query = new GetLeaderboardsQuery { Id = id };
            var handler = new GetLeaderboardsQueryHandler(GetFaultyDbContext());

            var leaderboard = new Leaderboard { Title = "Test list" };
            await DbContext.Leaderboards.AddAsync(leaderboard);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_todo_list()
        {
            // Arrange
            var query = new GetLeaderboardsQuery { Id = 1 };
            var handler = new GetLeaderboardsQueryHandler(DbContext);

            var leaderboard = new Leaderboard { Title = "Test list" };
            await DbContext.Leaderboards.AddAsync(leaderboard);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_todo_list_does_not_exist(int id)
        {
            // Arrange
            var query = new GetLeaderboardsQuery { Id = id };
            var handler = new GetLeaderboardsQueryHandler(DbContext);

            var leaderboard = new Leaderboard { Title = "Test list" };
            await DbContext.Leaderboards.AddAsync(leaderboard);
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
                new LeaderboardsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (LeaderboardsQuery)null;
            var handler = new LeaderboardsQueryHandler(DbContext);

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
            var query = new LeaderboardsQuery { Page = page, PageSize = pageSize };
            var handler = new LeaderboardsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_todo_lists()
        {
            // Arrange
            var query = new LeaderboardsQuery { Page = 1, PageSize = 5 };
            var handler = new LeaderboardsQueryHandler(DbContext);

            foreach(var i in Enumerable.Range(1, 15))
            {
                var leaderboard = new Leaderboard { Title = $"Test list {i}" };
                await DbContext.Leaderboards.AddAsync(leaderboard);
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
        public async Task List_should_return_empty_result_if_todo_lists_doesnt_exist()
        {
            // Arrange
            var query = new LeaderboardsQuery { Page = 1, PageSize = 5 };
            var handler = new LeaderboardsQueryHandler(DbContext);

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
                new DeleteLeaderboardsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteLeaderboardsCommand)null;
            var handler = new DeleteLeaderboardsCommandHandler(DbContext);

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
            var query = new DeleteLeaderboardsCommand { Id = id };
            var handler = new DeleteLeaderboardsCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_todo_list()
        {
            // Arrange
            var query = new DeleteLeaderboardsCommand { Id = 1 };
            var handler = new DeleteLeaderboardsCommandHandler(DbContext);

            var leaderboard = new Leaderboard { Title = "Test list" };
            await DbContext.Leaderboards.AddAsync(leaderboard);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Leaderboards.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_list()
        {
            // Arrange
            var query = new DeleteLeaderboardsCommand { Id = 1034 };
            var handler = new DeleteLeaderboardsCommandHandler(DbContext);

            var leaderboard = new Leaderboard { Title = "Test list" };
            await DbContext.Leaderboards.AddAsync(leaderboard);
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
                new SaveLeaderboardsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveLeaderboardsCommand)null;
            var handler = new SaveLeaderboardsCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_if_id_is_negative()
        {
            // Arrange
            var request = new SaveLeaderboardsCommand { Id = -10 };
            var handler = new SaveLeaderboardsCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_list()
        {
            // Arrange
            var request = new SaveLeaderboardsCommand { Id = 0, Title = "New list" };
            var handler = new SaveLeaderboardsCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedList = await DbContext.Leaderboards.SingleOrDefaultAsync(list => list.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedList);
            Assert.Equal(1, savedList.Id);
        }

        [Fact]
        public async Task Save_should_update_existing_list()
        {
            // Arrange
            var request = new SaveLeaderboardsCommand { Id = 1, Title = "Updated list" };
            var handler = new SaveLeaderboardsCommandHandler(DbContext);

            var leaderboard = new Leaderboard { Id = 0, Title = "New list" };
            await DbContext.Leaderboards.AddAsync(leaderboard);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedList = await DbContext.Leaderboards.SingleOrDefaultAsync(list => list.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedList);
            Assert.Equal(request.Title, savedList.Title);
        }

        [Fact]
        public async Task Save_should_not_update_missing_list()
        {
            // Arrange
            var request = new SaveLeaderboardsCommand { Id = 20, Title = "Updated list" };
            var handler = new SaveLeaderboardsCommandHandler(DbContext);

            var leaderboard = new Leaderboard { Id = 0, Title = "New list" };
            await DbContext.Leaderboards.AddAsync(leaderboard);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567890123456789012345678901234567890123456789000")]
        public void SaveValidator_should_return_false_when_title_is_invalid(string title)
        {
            // Arrange
            var validator = new SaveLeaderboardsCommandValidator(DbContext);
            var command = new SaveLeaderboardsCommand { Id = 0, Title = title };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid); 
            Assert.Equal(nameof(SaveLeaderboardsCommand.Title), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_title_is_valid()
        {
            // Arrange
            var validator = new SaveLeaderboardsCommandValidator(DbContext);
            var command = new SaveLeaderboardsCommand { Id = 0, Title = "ToDo list 1" };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}