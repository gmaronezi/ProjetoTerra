using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ProjetoTerra.Domain.Users.Entities;

public class ApplicationUser : IdentityUser<ObjectId>
{
    
}

public class CustomMongoUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IQueryableUserStore<ApplicationUser>, IDisposable
{
    private readonly IMongoCollection<ApplicationUser> _usersCollection;

    public CustomMongoUserStore(IMongoDatabase database)
    {
        _usersCollection = database.GetCollection<ApplicationUser>(nameof(ApplicationUser));
    }

    public void Dispose()
    {
        // Implemente a lógica para liberar recursos se necessário
    }

    // Implemente os métodos da interface IUserStore<ApplicationUser> e IUserPasswordStore<ApplicationUser> aqui
    public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.Id.ToString());
    }

    public async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(x => x.Id, ObjectId.Parse(userId));
        return _usersCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(x => x.NormalizedUserName, normalizedUserName);
        return _usersCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        var filter = Builders<ApplicationUser>.Filter.Eq(x => x.Id, user.Id);
        var update = Builders<ApplicationUser>.Update.Set(x => x.PasswordHash, passwordHash);
        return _usersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }

    public IQueryable<ApplicationUser> Users => _usersCollection.AsQueryable();
}