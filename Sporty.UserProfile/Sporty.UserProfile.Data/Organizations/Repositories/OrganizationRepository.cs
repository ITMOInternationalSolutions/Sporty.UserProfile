using Microsoft.EntityFrameworkCore;
using Sporty.UserProfile.Core.Organizations;
using Sporty.UserProfile.Core.Organizations.Repositories;
using Sporty.UserProfile.Core.Users;
using Sporty.UserProfile.Data.Users;
using Sporty.UserProfile.Domain.Exceptions;

namespace Sporty.UserProfile.Data.Organizations.Repositories;

public class OrganizationRepository : IOrganizationsRepository
{
    private readonly SportyContext _context;

    public OrganizationRepository(SportyContext context)
    {
        _context = context;
    }

    public async Task<Organization> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Organizations
            .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException($"Organization with id = {id} does not exists");
        }

        return new Organization
        {
            Id = entity.Id,
            Members = entity.Members?.Select(m => new User
            {
                Email = m.Email,
                Id = m.Id
            }).ToList(),
            Name = entity.Name,
            Organizers = entity.Organizers?.Select(org => new User
            {
                Email = org.Email,
                Id = org.Id
            }).ToList()
        };
    }

    Task<List<Organization>> IOrganizationsRepository.GetAllAsync(CancellationToken cancellationToken)
    {
        return _context.Organizations.Select(o => new Organization
        {
            Id = o.Id,
            Members = o.Members.Select(m => new User
            {
                Email = m.Email,
                Id = m.Id
            }).ToList(),
            Name = o.Name,
            Organizers = o.Organizers.Select(org => new User
            {
                Email = org.Email,
                Id = org.Id
            }).ToList()
        }).ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(Organization organization, CancellationToken cancellationToken)
    {
        var entity = new OrganizationDbModel
        {
            Id = organization.Id,
            Members = organization.Members?.Select(m => new Member
            {
                Email = m.Email,
                Id = m.Id
            }).ToList(),
            Name = organization.Name,
            Organizers = organization.Organizers?.Select(m => new Organizer
            {
                Email = m.Email,
                Id = m.Id
            }).ToList()
        };

        _context.Organizations.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Organization organization, CancellationToken cancellationToken)
    {
        var entity = new OrganizationDbModel
        {
            Members = organization.Members?.Select(m => new Member
            {
                Email = m.Email,
                Id = m.Id
            }).ToList(),
            Name = organization.Name,
            Organizers = organization.Organizers?.Select(m => new Organizer
            {
                Email = m.Email,
                Id = m.Id
            }).ToList()
        };

        _context.Organizations.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Organizations.FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException($"Organization with id = {id} does not exists");
        }

        _context.Organizations.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}