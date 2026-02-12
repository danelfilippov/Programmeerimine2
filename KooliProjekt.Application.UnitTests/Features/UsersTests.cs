using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class UsersTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetUsersQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetUsersQuery)null;
            var handler = new GetUsersQueryHandler(DbContext);

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
            var query = new GetUsersQuery { Id = id };
            var handler = new GetUsersQueryHandler(GetFaultyDbContext());

            var user = new User { Title = "Test user" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_user()
        {
            // Arrange
            var query = new GetUsersQuery { Id = 1 };
            var handler = new GetUsersQueryHandler(DbContext);

            var user = new User { Title = "Test user" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, ((User)result.Value).Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_user_does_not_exist(int id)
        {
            // Arrange
            var query = new GetUsersQuery { Id = id };
            var handler = new GetUsersQueryHandler(DbContext);

            var user = new User { Title = "Test user" };
            await DbContext.Users.AddAsync(user);
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
                new UsersQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (UsersQuery)null;
            var handler = new UsersQueryHandler(DbContext);

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
            var query = new UsersQuery { Page = page, PageSize = pageSize };
            var handler = new UsersQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_users()
        {
            // Arrange
            var query = new UsersQuery { Page = 1, PageSize = 5 };
            var handler = new UsersQueryHandler(DbContext);

            foreach(var i in Enumerable.Range(1, 15))
            {
                var user = new User { Title = $"Test user {i}" };
                await DbContext.Users.AddAsync(user);
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
        public async Task List_should_return_empty_result_if_users_doesnt_exist()
        {
            // Arrange
            var query = new UsersQuery { Page = 1, PageSize = 5 };
            var handler = new UsersQueryHandler(DbContext);

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
                new DeleteUsersCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteUsersCommand)null;
            var handler = new DeleteUsersCommandHandler(DbContext);

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
            var query = new DeleteUsersCommand { Id = id };
            var handler = new DeleteUsersCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_user()
        {
            // Arrange
            var query = new DeleteUsersCommand { Id = 1 };
            var handler = new DeleteUsersCommandHandler(DbContext);

            var user = new User { Title = "Test user" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Users.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_user()
        {
            // Arrange
            var query = new DeleteUsersCommand { Id = 1034 };
            var handler = new DeleteUsersCommandHandler(DbContext);

            var user = new User { Title = "Test user" };
            await DbContext.Users.AddAsync(user);
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
                new SaveUsersCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveUsersCommand)null;
            var handler = new SaveUsersCommandHandler(DbContext);

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
            var request = new SaveUsersCommand { Id = -10 };
            var handler = new SaveUsersCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "Id");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_user()
        {
            // Arrange
            var request = new SaveUsersCommand { Id = 0, Title = "New user" };
            var handler = new SaveUsersCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedUser = await DbContext.Users.SingleOrDefaultAsync(u => u.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedUser);
            Assert.Equal(1, savedUser.Id);
        }

        [Fact]
        public async Task Save_should_save_existing_user()
        {
            // Arrange
            var userToAdd = new User { Id = 0, Title = "New user" };
            var request = new SaveUsersCommand { Id = 1, Title = "Updated user" };
            var handler = new SaveUsersCommandHandler(DbContext);

            await DbContext.Users.AddAsync(userToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedUser = await DbContext.Users.SingleOrDefaultAsync(u => u.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedUser);
            Assert.Equal(request.Title, savedUser.Title);
        }

        [Fact]
        public async Task Save_should_return_error_if_user_does_not_exist()
        {
            // Arrange
            var userToAdd = new User { Id = 0, Title = "New user" };
            var request = new SaveUsersCommand { Id = 8, Title = "Updated user" };
            var handler = new SaveUsersCommandHandler(DbContext);

            await DbContext.Users.AddAsync(userToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}
