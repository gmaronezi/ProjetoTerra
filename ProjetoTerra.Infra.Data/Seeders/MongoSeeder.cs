using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjetoTerra.Domain.Users.Entities;

namespace ProjetoTerra.Infra.Data.Seeders;

public class MongoSeeder
{
    private readonly IMongoDatabase _database;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly UserManager<ApplicationUser> _userManager;

    public MongoSeeder(IMongoDatabase database, IPasswordHasher<ApplicationUser> passwordHasher, UserManager<ApplicationUser> userManager)
    {
        _database = database;
        _passwordHasher = passwordHasher;
        _userManager = userManager;
    }
    
    public async Task SeedUserTest()
    {
        var email = "teste@teste.com.br";

        var user = await _userManager.FindByNameAsync(email);
        
        if (user == null)
        {
            // Usuário criado para teste
            user = new ApplicationUser()
            {
                Id = ObjectId.GenerateNewId(),
                Email = "teste@teste.com.br",
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = false,
                UserName = "teste@teste.com.br",
                NormalizedUserName = email.ToUpper(),
                TwoFactorEnabled = false,
            };
        
            var password = "senha123";
            var hashedPassword = _passwordHasher.HashPassword(user, password);
            user.PasswordHash = hashedPassword;
        
            await _database.GetCollection<ApplicationUser>(nameof(ApplicationUser)).InsertOneAsync(user);
        }
    }
}