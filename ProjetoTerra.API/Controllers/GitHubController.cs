using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTerra.Application.GitHub.Commands;
using ProjetoTerra.Application.GitHub.Queries;
using ProjetoTerra.Shared.Abstractions;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v1/github")]
[Authorize]
public class GitHubController : ControllerBase
{
    private readonly IMediator _mediator;

    public GitHubController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("repository")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailedResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateRepository([FromBody] CreateGitRepositoryCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    
    [HttpPost("webhook/{repositoryName}")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailedResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateWebhook([FromRoute] string repositoryName, [FromBody] CreateWebhookCommand command)
    {
        var result = await _mediator.Send(command.SetRepositoryName(repositoryName));

        return Ok(result);
    }
    
    [HttpPut("webhook/{repositoryName}/{webHookId}")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailedResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateWebhook(
        [FromRoute(Name = "repositoryName")] string repositoryName, 
        [FromRoute(Name = "webHookId")] int webHookId, 
        [FromBody] UpdateWebhookCommand command)
    {
        command.SetIdRepositoryName(webHookId, repositoryName);
        var result = await _mediator.Send(command);

        return Ok(result);
    }
    
    [HttpGet("branchs/{repositoryName}")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailedResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetBranchs(
        string repositoryName,
        [FromQuery(Name = "pagina")] int page = 1,
        [FromQuery(Name = "pageSize")] int pageSize = 25)
    {
        var result = await _mediator.Send(new GetBranchsQuery(repositoryName, page, pageSize));

        return Ok(result);
    }
    
    [HttpGet("webhooks/{repositoryName}")]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailedResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IActionResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetWebhooks(
        string repositoryName,
        [FromQuery(Name = "pagina")] int page = 1,
        [FromQuery(Name = "pageSize")] int pageSize = 25)
    {
        var result = await _mediator.Send(new GetWebhooksQuery(repositoryName, page, pageSize));

        return Ok(result);
    }
}