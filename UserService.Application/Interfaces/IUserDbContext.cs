using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IUserDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserFavorite> UserFavorites { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
