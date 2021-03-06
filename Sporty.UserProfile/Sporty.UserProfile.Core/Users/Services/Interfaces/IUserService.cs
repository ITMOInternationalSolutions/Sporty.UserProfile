using Sporty.UserProfile.Core.Users.Entities;

namespace Sporty.UserProfile.Core.Users.Services.Interfaces;

public interface IUserService
{
    Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    Task<string> RegisterAsync(User user, CancellationToken cancellationToken);
    Task<string> LoginAsync(User user, CancellationToken cancellationToken);
}