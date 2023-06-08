using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MongoDB.Bson;
using ProjetoTerra.Domain.Users.Entities;

namespace ProjetoTerra.Infra.Data;

public class ApplicationUserStore : UserStore<ApplicationUser, UserRole, ApplicationDbContext, ObjectId>
{
    public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
    {
    }
}