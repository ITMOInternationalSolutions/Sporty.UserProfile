using Sporty.UserProfile.Data.Users;

namespace Sporty.UserProfile.Data.Organizations;

public class OrganizationDbModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public virtual List<Member>? Members { get; set; }
    public virtual List<Organizer>? Organizers { get; set; }
}