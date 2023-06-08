using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjetoTerra.Application.Users.Commands;
using ProjetoTerra.Application.Users.Services;
using ProjetoTerra.Application.Users.ViewModels;
using ProjetoTerra.Domain.Users.Entities;
using ProjetoTerra.Shared.Exceptions;
using ProjetoTerra.Shared.Helpers;

namespace ProjetoTerra.Application.Users.CommandHandlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthTokenViewModel>
{
    private readonly IUserAuthService _userAuthService;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginCommandHandler(UserManager<ApplicationUser> userManager, IUserAuthService userAuthService)
    {
        _userManager = userManager;
        _userAuthService = userAuthService;
    }

    public async Task<AuthTokenViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new InvalidActionException(ResourceHelper.UserNotFound);
        }
        
        return await _userAuthService.GenerateTokenByUser(user);
    }
}