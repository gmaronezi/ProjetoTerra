using Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Octokit;
using ProjetoTerra.Application.GitHub.Commands;
using ProjetoTerra.Application.GitHub.Queries;
using ProjetoTerra.Application.GitHub.ViewModels;

namespace ProjetoTerra.Tests;

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
            var command = new CreateGitRepositoryCommand()
            {
                RepositoryName = RepositoryName,
                Description = "Teste",
                Private = true
            };
            
            var expectedResult = true;
            _mediatorMock.Setup(x => x.Send(command, default(CancellationToken)))
                .ReturnsAsync(expectedResult);

            var result = await _controller.CreateRepository(command);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void CreateWebhook_ValidRequest_ReturnsOk()
        {
            var command = new CreateWebhookCommand()
            {
                Name = "web",
                URLCallback = "https://teste.teste.com.br",
                Events = new[]
                {
                    "push"
                },
                Config = new Dictionary<string, string>()
                {
                    { "url", "https://teste2.teste.com.br" }
                },
                Secret = "teste_teste"
            };
            
            var expectedResult = true;
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateWebhookCommand>(), default(CancellationToken)))
                .ReturnsAsync(expectedResult);

            var result = await _controller.CreateWebhook(RepositoryName, command);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void UpdateWebhook_ValidRequest_ReturnsOk()
        {
            var webHookId = 1;
            var command = new UpdateWebhookCommand()
            {
                Active = true,
                Events = new[]
                {
                    "push"
                },
                Config = new Dictionary<string, string>()
                {
                    { "url", "https://teste3.teste.com.br" }
                }
            };
            
            var expectedResult = true;
            _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateWebhookCommand>(), default(CancellationToken)))
                .ReturnsAsync(expectedResult);

            var result = await _controller.UpdateWebhook(RepositoryName, webHookId, command);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetBranchs_ValidRequest_ReturnsOk()
        {
            var page = 1;
            var pageSize = 25;
            var expectedResult = new List<BranchViewModel>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetBranchsQuery>(), default(CancellationToken)))
                .ReturnsAsync(expectedResult);

            var result = await _controller.GetBranchs(RepositoryName, page, pageSize);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetWebhooks_ValidRequest_ReturnsOk()
        {
            var repositoryName = "test-repo";
            var page = 1;
            var pageSize = 25;
            var expectedResult = new List<WebhookViewModel>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetWebhooksQuery>(), default(CancellationToken)))
                .ReturnsAsync(expectedResult);
        
            var result = await _controller.GetWebhooks(repositoryName, page, pageSize);
        
            Assert.IsType<OkObjectResult>(result);
        }
}