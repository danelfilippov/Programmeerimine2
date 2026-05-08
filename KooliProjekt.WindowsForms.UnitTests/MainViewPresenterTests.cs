using KooliProjekt.WindowsForms.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    public class MainViewPresenterTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IMainView> _mainViewMock;
        private readonly MainViewPresenter _presenter;

        public MainViewPresenterTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _mainViewMock = new Mock<IMainView>();
            _presenter = new MainViewPresenter(_apiClientMock.Object, _mainViewMock.Object);
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult<PagedResult<User>>();
            faultyResponse.Errors.Add("An error occurred while fetching data.");

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = null)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult<PagedResult<User>>
            {
                Value = new PagedResult<User>
                {
                    Results = new List<User>
                    {
                        new User { Id = 1, Title = "Test List 1" },
                        new User { Id = 2, Title = "Test List 2" }
                    }
                }
            };

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(validResponse)
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = validResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_clear_fields_with_null_selection()
        {
            // Arrange
            var selectedList = (User)null;

            _mainViewMock.SetupSet(view => view.CurrentId = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentTitle = "").Verifiable();

            // Act
            _presenter.SetSelection(selectedList);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_set_fields_with_valid_selection()
        {
            // Arrange
            var selectedUser = new User { Id = 1, Title = "Test User" };

            _mainViewMock.SetupSet(view => view.CurrentId = 1).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentTitle = "Test User").Verifiable();

            // Act
            _presenter.SetSelection(selectedUser);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult();
            faultyResponse.Errors.Add("An error occurred while saving data.");

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<User>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult();

            _mainViewMock.Setup(view => view.CurrentId).Returns(1);
            _mainViewMock.Setup(view => view.CurrentTitle).Returns("Test User");

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<User>()))
                .ReturnsAsync(validResponse)
                .Verifiable();

            var listResponse = new OperationResult<PagedResult<User>>
            {
                Value = new PagedResult<User>
                {
                    Results = new List<User> { new User { Id = 1, Title = "Test User" } }
                }
            };

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(listResponse)
                .Verifiable();

            _mainViewMock
                .SetupSet(view => view.DataSource = listResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            // Arrange
            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(false)
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult();
            faultyResponse.Errors.Add("An error occurred while deleting data.");

            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(true)
                .Verifiable();

            _apiClientMock
                .Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();

            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult();

            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(true)
                .Verifiable();

            _mainViewMock
                .Setup(view => view.CurrentId)
                .Returns(1);

            _apiClientMock
                .Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(validResponse)
                .Verifiable();

            var listResponse = new OperationResult<PagedResult<User>>
            {
                Value = new PagedResult<User>
                {
                    Results = new List<User>()
                }
            };

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(listResponse)
                .Verifiable();

            _mainViewMock
                .SetupSet(view => view.DataSource = listResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }
    }
}