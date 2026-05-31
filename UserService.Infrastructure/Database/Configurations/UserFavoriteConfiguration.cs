using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Entities;

namespace UserService.Infrastructure.Database.Configurations;

public class UserFavoriteConfiguration : IEntityTypeConfiguration<UserFavorite>
{
    public void Configure(EntityTypeBuilder<UserFavorite> builder)
    {
        builder.HasKey(uf => new { uf.UserId, uf.CurrencyId });

        builder.HasOne<UserService.Domain.Entities.User>()
            .WithMany(u => u.Favorites)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
