using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjetoTerra.Application.GitHub.Commands;
using ProjetoTerra.Application.GitHub.Queries;
using Xunit;

namespace Api.Controllers
{
    public class GitHubControllerTests
    {
        private readonly GitHubController _controller;
        private readonly Mock<IMediator> _mediatorMock;
        private const string RepositoryName = "teste";

        public GitHubControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new GitHubController(_mediatorMock.Object);
        }

        [Fact]
        public async void CreateRepository_ValidRequest_ReturnsOk()
        {
            // Arrange
            var command = new CreateGitRepositoryCommand();
            var expectedResult = true;
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateGitRepositoryCommand>(), default(CancellationToken)))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateRepository(RepositoryName, command);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreateWebhook_ValidRequest_ReturnsOk()
        {
            // Arrange
            var command = new CreateWebhookCommand();
            var expectedResult = true;
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateWebhookCommand>(), default(CancellationToken)))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateWebhook(RepositoryName, command);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void UpdateWebhook_ValidRequest_ReturnsOk()
        {
            // Arrange
            var webHookId = 1;
            var command = new UpdateWebhookCommand();
            var expectedResult = true;
            _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateWebhookCommand>(), default(CancellationToken)))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UpdateWebhook(RepositoryName, webHookId, command);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetBranchs_ValidRequest_ReturnsOk()
        {
            // Arrange
            var page = 1;
            var pageSize = 25;
            var expectedResult = new List<string>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetBranchsQuery>(), default(CancellationToken)))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetBranchs(RepositoryName, page, pageSize);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        // [Fact]
        // public async void GetWebhooks_ValidRequest_ReturnsOk()
        // {
        //     // Arrange
        //     var repositoryName = "test-repo";
        //     var page = 1;
        //     var pageSize = 25;
        //     var query = new GetWebhooksQuery(repositoryName, page, pageSize);
        //     var expectedResult = new OkResult();
        //     _mediatorMock.Setup(x => x.Send(It.IsAny<GetWebhooksQuery>(), default(CancellationToken)))
        //         .ReturnsAsync(expectedResult);
        //
        //     // Act
        //     var result = await _controller.GetWebhooks(repositoryName, page, pageSize);
        //
        //     // Assert
        //     Assert.IsType<OkResult>(result);
        // }
    }
}
