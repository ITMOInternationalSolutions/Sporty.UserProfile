﻿using Sporty.UserProfile.Core.Organizations.Repositories;
using Sporty.UserProfile.Core.Organizations.Services.Interfaces;
using Sporty.UserProfile.Core.Users;
using Sporty.UserProfile.Core.Users.Repositories;

namespace Sporty.UserProfile.Core.Organizations.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationsRepository _organizationsRepository;
    private readonly IUserRepository _userRepository;

    public OrganizationService(IOrganizationsRepository organizationsRepository, IUserRepository userRepository)
    {
        _organizationsRepository = organizationsRepository;
        _userRepository = userRepository;
    }

    public Task<Organization> GetByIdAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        return _organizationsRepository.GetByIdAsync(organizationId, cancellationToken);
    }

    public async Task<IReadOnlyList<Organization>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var organizations = await _organizationsRepository.GetAllAsync(cancellationToken);

        return organizations.Where(o => 
            o.Members.Any(m => m.Id == userId) ||
            o.Organizers.Any(org => org.Id == userId)).ToList();
    }

    public async Task<IReadOnlyList<Organization>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var organizations = await _organizationsRepository.GetAllAsync(cancellationToken);

        return organizations.Where(o => o.Name.Contains(name)).ToList();
    }

    public Task<IReadOnlyList<Organization>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _organizationsRepository.GetAllAsync(cancellationToken);
    }

    public async Task CreateAsync(string name, Guid creatorId, CancellationToken cancellationToken)
    {
        var creator = await _userRepository.GetByIdAsync(creatorId, cancellationToken);

        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            Members = new List<User>(),
            Name = name,
            Organizers = new List<User>(new[] { creator })
        };

        await _organizationsRepository.CreateAsync(organization, cancellationToken);
    }

    public async Task AddMemberAsync(Guid organizationId, Guid memberId, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.GetByIdAsync(organizationId, cancellationToken);
        var member = await _userRepository.GetByIdAsync(memberId, cancellationToken);
        
        organization.Members.Add(member);

        await _organizationsRepository.UpdateAsync(organization, cancellationToken);
    }

    public async Task AddOrganizer(Guid organizationId, Guid organizerId, CancellationToken cancellationToken)
    {
        var organization = await _organizationsRepository.GetByIdAsync(organizationId, cancellationToken);
        var organizer = await _userRepository.GetByIdAsync(organizerId, cancellationToken);
        
        organization.Organizers.Add(organizer);

        await _organizationsRepository.UpdateAsync(organization, cancellationToken);
    }

    public Task DeleteAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        return _organizationsRepository.DeleteAsync(organizationId, cancellationToken);
    }
}