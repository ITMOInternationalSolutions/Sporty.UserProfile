using Sporty.UserProfile.Core.Users;

namespace Sporty.UserProfile.Core.Organizations.Services.Interfaces;

public interface IOrganizationService
{
    Task<Organization> GetByIdAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Organization>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Organization>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Organization>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(string name, Guid creatorId, CancellationToken cancellationToken);
    Task AddMemberAsync(Guid organizationId, Guid memberId, CancellationToken cancellationToken);
    Task AddOrganizer(Guid organizationId, Guid organizerId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid organizationId, CancellationToken cancellationToken);
}