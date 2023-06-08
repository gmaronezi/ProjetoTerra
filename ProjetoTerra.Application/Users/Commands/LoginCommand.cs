using MediatR;
using ProjetoTerra.Application.Users.ViewModels;

namespace ProjetoTerra.Application.Users.Commands;

public class LoginCommand : IRequest<AuthTokenViewModel>
{
    public string Username { get; set; }
    public string Password { get; set; }
}