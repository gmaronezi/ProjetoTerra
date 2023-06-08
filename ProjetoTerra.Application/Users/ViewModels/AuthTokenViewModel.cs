namespace ProjetoTerra.Application.Users.ViewModels;

public class AuthTokenViewModel
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public DateTime? ExpiresAt { get; set; }
}