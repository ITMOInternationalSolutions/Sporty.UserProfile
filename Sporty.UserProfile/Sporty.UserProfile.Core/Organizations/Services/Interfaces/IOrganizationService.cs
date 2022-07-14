using Sporty.UserProfile.Core.Users;

namespace Sporty.UserProfile.Core.Organizations.Services.Interfaces;

public interface IOrganizationService
{
    Task<Organization> GetByIdAsync(Guid organizationId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Organization>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Organization>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<List<Organization>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(string name, Guid creatorId, CancellationToken cancellationToken);
    Task AddMemberAsync(Guid organizationId, string memberEmail, CancellationToken cancellationToken);
    Task AddOrganizerAsync(Guid organizationId, string organizerEmail, CancellationToken cancellationToken);
    Task DeleteAsync(Guid organizationId, CancellationToken cancellationToken);
}