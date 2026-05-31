using Shared.Domain.Entities;

namespace UserService.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public byte[] Password { get; set; } = [];
    public ICollection<UserFavorite> Favorites { get; set; } = [];
}
