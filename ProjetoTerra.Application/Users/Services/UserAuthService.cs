using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjetoTerra.Application.Users.ViewModels;
using ProjetoTerra.Domain.Users.Entities;

namespace ProjetoTerra.Application.Users.Services;

public class UserAuthService : IUserAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public UserAuthService(
        SignInManager<ApplicationUser> signInManager, 
        IConfiguration configuration)
    {
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<AuthTokenViewModel> GenerateTokenByUser(ApplicationUser applicationUser)
    {
        var (accessToken, expiresAt) = GenerateToken(applicationUser);
        await _signInManager.SignInAsync(applicationUser, false);

        return new AuthTokenViewModel()
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            ExpiresAt = expiresAt
        };
    }
    
    private (string token, DateTime expiresAt) GenerateToken(ApplicationUser applicationUser)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
        var expiresAt = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:AccessTokenExpirationMinutes"]));

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()),
            new Claim(ClaimTypes.Name, applicationUser.UserName)
        });
        
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claimsIdentity,
            Expires = expiresAt,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), expiresAt);
    }
}