using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Predictions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class PredictionsTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetPredictionsQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetPredictionsQuery)null;
            var handler = new GetPredictionsQueryHandler(DbContext);

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
            var query = new GetPredictionsQuery { Id = id };
            var handler = new GetPredictionsQueryHandler(GetFaultyDbContext());

            var prediction = new Prediction { Title = "Test prediction" };
            await DbContext.Predictions.AddAsync(prediction);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_prediction()
        {
            // Arrange
            var query = new GetPredictionsQuery { Id = 1 };
            var handler = new GetPredictionsQueryHandler(DbContext);

            var prediction = new Prediction { Title = "Test prediction" };
            await DbContext.Predictions.AddAsync(prediction);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, ((Prediction)result.Value).Id);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_prediction_does_not_exist(int id)
        {
            // Arrange
            var query = new GetPredictionsQuery { Id = id };
            var handler = new GetPredictionsQueryHandler(DbContext);

            var prediction = new Prediction { Title = "Test prediction" };
            await DbContext.Predictions.AddAsync(prediction);
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
                new PredictionsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (PredictionsQuery)null;
            var handler = new PredictionsQueryHandler(DbContext);

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
            var query = new PredictionsQuery { Page = page, PageSize = pageSize };
            var handler = new PredictionsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_predictions()
        {
            // Arrange
            var query = new PredictionsQuery { Page = 1, PageSize = 5 };
            var handler = new PredictionsQueryHandler(DbContext);

            foreach(var i in Enumerable.Range(1, 15))
            {
                var prediction = new Prediction { Title = $"Test prediction {i}" };
                await DbContext.Predictions.AddAsync(prediction);
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
        public async Task List_should_return_empty_result_if_predictions_doesnt_exist()
        {
            // Arrange
            var query = new PredictionsQuery { Page = 1, PageSize = 5 };
            var handler = new PredictionsQueryHandler(DbContext);

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
                new DeletePredictionsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeletePredictionsCommand)null;
            var handler = new DeletePredictionsCommandHandler(DbContext);

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
            var query = new DeletePredictionsCommand { Id = id };
            var handler = new DeletePredictionsCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_prediction()
        {
            // Arrange
            var query = new DeletePredictionsCommand { Id = 1 };
            var handler = new DeletePredictionsCommandHandler(DbContext);

            var prediction = new Prediction { Title = "Test prediction" };
            await DbContext.Predictions.AddAsync(prediction);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Predictions.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_prediction()
        {
            // Arrange
            var query = new DeletePredictionsCommand { Id = 1034 };
            var handler = new DeletePredictionsCommandHandler(DbContext);

            var prediction = new Prediction { Title = "Test prediction" };
            await DbContext.Predictions.AddAsync(prediction);
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
                new SavePredictionsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SavePredictionsCommand)null;
            var handler = new SavePredictionsCommandHandler(DbContext);

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
            var request = new SavePredictionsCommand { Id = -10 };
            var handler = new SavePredictionsCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "Id");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_prediction()
        {
            // Arrange
            var request = new SavePredictionsCommand { Id = 0, Title = "New prediction" };
            var handler = new SavePredictionsCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedPrediction = await DbContext.Predictions.SingleOrDefaultAsync(p => p.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedPrediction);
            Assert.Equal(1, savedPrediction.Id);
        }

        [Fact]
        public async Task Save_should_save_existing_prediction()
        {
            // Arrange
            var predictionToAdd = new Prediction { Id = 0, Title = "New prediction" };
            var request = new SavePredictionsCommand { Id = 1, Title = "Updated prediction" };
            var handler = new SavePredictionsCommandHandler(DbContext);

            await DbContext.Predictions.AddAsync(predictionToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedPrediction = await DbContext.Predictions.SingleOrDefaultAsync(p => p.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedPrediction);
            Assert.Equal(request.Title, savedPrediction.Title);
        }

        [Fact]
        public async Task Save_should_return_error_if_prediction_does_not_exist()
        {
            // Arrange
            var predictionToAdd = new Prediction { Id = 0, Title = "New prediction" };
            var request = new SavePredictionsCommand { Id = 8, Title = "Updated prediction" };
            var handler = new SavePredictionsCommandHandler(DbContext);

            await DbContext.Predictions.AddAsync(predictionToAdd);
            await DbContext.SaveChangesAsync();

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }
    }
}
