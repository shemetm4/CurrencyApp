using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Entities;

namespace FinanceService.Infrastructure.Database.Configurations;

public class UserFavoriteConfiguration : IEntityTypeConfiguration<UserFavorite>
{
    public void Configure(EntityTypeBuilder<UserFavorite> builder)
    {
        builder.HasKey(uf => new { uf.UserId, uf.CurrencyId });
    }
}
