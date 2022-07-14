using Sporty.UserProfile.Core.Users;

namespace Sporty.UserProfile.Core.Organizations;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual List<User> Members { get; set; }
    public virtual List<User> Organizers { get; set; }
}