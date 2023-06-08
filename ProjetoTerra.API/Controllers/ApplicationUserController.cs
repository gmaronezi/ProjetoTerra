using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoTerra.Application.Users.Commands;
using ProjetoTerra.Application.Users.ViewModels;
using ProjetoTerra.Shared.Abstractions;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v1/auth")]
[Authorize]
public class ApplicationUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationUserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthTokenViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailedResult), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}