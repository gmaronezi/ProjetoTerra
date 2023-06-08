using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjetoTerra.Domain.Users.Entities;

namespace ProjetoTerra.Infra.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, UserRole, ObjectId>
{
    private readonly IMongoDatabase _database;

    public ApplicationDbContext(IMongoDatabase database)
    {
        _database = database;
    }
    
    public IMongoDatabase MongoDatabase => _database;

    public IMongoCollection<ApplicationUser> ApplicationUsers => _database.GetCollection<ApplicationUser>(nameof(ApplicationUser));
}