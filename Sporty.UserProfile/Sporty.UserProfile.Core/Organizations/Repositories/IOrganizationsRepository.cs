using Sporty.UserProfile.Core.Users;

namespace Sporty.UserProfile.Core.Organizations.Repositories;

public interface IOrganizationsRepository
{
    Task<Organization> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Organization>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(Organization organization, CancellationToken cancellationToken);
    Task AddMember(Guid organizationId, User member, CancellationToken cancellationToken);
    Task AddOrganizer(Guid organizationId, User organizer, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}