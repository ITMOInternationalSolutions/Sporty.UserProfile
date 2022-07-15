using Sporty.UserProfile.Web.Controllers.Dto;
namespace Sporty.UserProfile.Web.Controllers.Organizations.Dto;

public class OrganizationDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<UserDto>? Members { get; set; }
    public List<UserDto>? Organizers { get; set; }
}