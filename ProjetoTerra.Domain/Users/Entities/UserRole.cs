using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ProjetoTerra.Domain.Users.Entities;

public class UserRole : IdentityRole<ObjectId>
{
    
}

public class CustomRoleStore : IRoleStore<IdentityRole>
{
    private readonly IMongoCollection<IdentityRole> _rolesCollection;

    public CustomRoleStore(IMongoDatabase database)
    {
        _rolesCollection = database.GetCollection<IdentityRole>(nameof(UserRole));
    }

    public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        // Implemente a lógica para criar uma nova função
        throw new InvalidOperationException();
    }

    public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        // Implemente a lógica para excluir uma função
        throw new InvalidOperationException();
    }

    public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        // Implemente a lógica para buscar uma função por ID
        throw new InvalidOperationException();
    }

    public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        // Implemente a lógica para buscar uma função pelo nome
        throw new InvalidOperationException();
    }

    public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        // Implemente a lógica para obter o nome normalizado da função
        throw new InvalidOperationException();
    }

    public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        // Implemente a lógica para obter o ID da função
        throw new InvalidOperationException();
    }

    public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        // Implemente a lógica para obter o nome da função
        throw new InvalidOperationException();
    }

    public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName,
        CancellationToken cancellationToken)
    {
        // Implemente a lógica para definir o nome normalizado da função
        throw new InvalidOperationException();
    }

    public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
    {
        // Implemente a lógica para definir o nome da função
        throw new InvalidOperationException();
    }

    public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        // Implemente a lógica para atualizar uma função
        throw new InvalidOperationException();
    }

    public void Dispose()
    {
        // Implemente a lógica para liberar recursos, se necessário
    }
}