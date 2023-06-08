using ProjetoTerra.Application.Users.ViewModels;
using ProjetoTerra.Domain.Users.Entities;

namespace ProjetoTerra.Application.Users.Services;

public interface IUserAuthService
{
    Task<AuthTokenViewModel> GenerateTokenByUser(ApplicationUser applicationUser);
}