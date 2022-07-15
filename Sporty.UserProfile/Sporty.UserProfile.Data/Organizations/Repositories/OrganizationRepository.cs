using Microsoft.EntityFrameworkCore;
using Sporty.UserProfile.Core.Organizations;
using Sporty.UserProfile.Core.Organizations.Repositories;
using Sporty.UserProfile.Core.Users;
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
            .AsNoTracking()
            .Include(o => o.Members)
            .Include(o => o.Organizers)
            .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException($"Organization with id = {id} does not exists");
        }

        return new Organization
        {
            Id = entity.Id,
            Members = entity.Members.Select(m => new User
            {
                Email = m.Email,
                Id = m.Id
            }).ToList(),
            Name = entity.Name,
            Organizers = entity.Organizers.Select(org => new User
            {
                Email = org.Email,
                Id = org.Id
            }).ToList()
        };
    }

    Task<List<Organization>> IOrganizationsRepository.GetAllAsync(CancellationToken cancellationToken)
    {
        return _context.Organizations
            .AsNoTracking()
            .Include(o => o.Members)
            .Include(o => o.Organizers)
            .Select(o => new Organization
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
            Members = organization.Members.Select(m => new Member
            {
                Email = m.Email,
                Id = m.Id
            }).ToList(),
            Name = organization.Name,
            Organizers = organization.Organizers.Select(m => new Organizer
            {
                Email = m.Email,
                Id = m.Id
            }).ToList()
        };

        _context.Organizations.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddMember(Guid organizationId, User member, CancellationToken cancellationToken)
    {
        var entity = await _context.Organizations
            .FirstOrDefaultAsync(o => o.Id == organizationId, cancellationToken: cancellationToken);
        if (entity is null)
        {
            throw new NotFoundException("There is no such organization.");
        }

        var members = entity.Members;
        members.Add(new Member
        {
            Email = member.Email,
            Id = member.Id
        });
        entity.Members = members;

        _context.Organizations.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddOrganizer(Guid organizationId, User organizer, CancellationToken cancellationToken)
    {
        var entity = await _context.Organizations
            .FirstOrDefaultAsync(o => o.Id == organizationId, cancellationToken: cancellationToken);
        if (entity is null)
        {
            throw new NotFoundException("There is no such organization.");
        }

        entity.Organizers.Add(new Organizer
        {
            Email = organizer.Email,
            Id = organizer.Id
        });

        _context.Organizations.Update(entity);
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