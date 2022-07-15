using Microsoft.AspNetCore.Mvc;
using Sporty.UserProfile.Core.Organizations.Services.Interfaces;
using Sporty.UserProfile.Domain.Exceptions;
using Sporty.UserProfile.Web.Controllers.Dto;
using Sporty.UserProfile.Web.Controllers.Organizations.Dto;
using Sporty.UserProfile.Web.Controllers.Users.Dto;
using Sporty.UserProfile.Web.Extensions;

namespace Sporty.UserProfile.Web.Controllers.Organizations;

[ApiController]
[Route("organization")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly ILogger<OrganizationController> _logger;
    private readonly IConfiguration _configuration;

    public OrganizationController(IOrganizationService organizationService, ILogger<OrganizationController> logger,
        IConfiguration configuration)
    {
        _organizationService = organizationService;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Return a organization model.
    /// </summary>
    /// <param name="organizationId">ID of target organization.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Organization model which contains: Name, Members, Organizers and ID.</returns>
    [HttpGet("get-by-id")]
    public async Task<OrganizationDto> GetById([FromQuery] Guid organizationId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to get organization");

        var organization = await _organizationService.GetByIdAsync(organizationId, cancellationToken);

        _logger.LogInformation("Organization successfully retrieved");
        return new OrganizationDto
        {
            Id = organization.Id,
            Members = organization.Members.Select(m => new UserDto { Email = m.Email, Id = m.Id }).ToList(),
            Name = organization.Name,
            Organizers = organization.Organizers.Select(org => new UserDto { Email = org.Email, Id = org.Id }).ToList()
        };
    }

    /// <summary>
    /// Return a organizations which have specified user if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Organization models which contain: Name, Members, Organizers and ID.</returns>
    [HttpGet("get-by-user-id")]
    public async Task<IReadOnlyList<OrganizationDto>> GetByUserId(
        [FromHeader(Name = "Token")] string token,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to get organizations");

        var userId = HttpExtensions.GetIdFromItems(HttpContext);
        var organizations = await _organizationService.GetByUserIdAsync(userId, cancellationToken);

        _logger.LogInformation("Organizations successfully retrieved");
        return organizations.Select(organization => new OrganizationDto
        {
            Id = organization.Id,
            Members = organization.Members.Select(m => new UserDto { Email = m.Email, Id = m.Id }).ToList(),
            Name = organization.Name,
            Organizers = organization.Organizers.Select(org => new UserDto { Email = org.Email, Id = org.Id }).ToList()
        }).ToList();
    }

    /// <summary>
    /// Return a organizations which contain specified name.
    /// </summary>
    /// <param name="name">Name of organization.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Organization models which contain: Name, Members, Organizers and ID.</returns>
    [HttpGet("get-by-name")]
    public async Task<IReadOnlyList<OrganizationDto>> GetByName(
        [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to get organizations");

        var organizations = await _organizationService.GetByNameAsync(name, cancellationToken);

        _logger.LogInformation("Organizations successfully retrieved");
        return organizations.Select(organization => new OrganizationDto
        {
            Id = organization.Id,
            Members = organization.Members.Select(m => new UserDto { Email = m.Email, Id = m.Id }).ToList(),
            Name = organization.Name,
            Organizers = organization.Organizers.Select(org => new UserDto { Email = org.Email, Id = org.Id }).ToList()
        }).ToList();
    }

    /// <summary>
    /// Return all organizations.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>Organization models which contain: Name, Members, Organizers and ID.</returns>
    [HttpGet("get-all")]
    public async Task<List<OrganizationDto>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to get organizations");

        var organizations = await _organizationService.GetAllAsync(cancellationToken);

        _logger.LogInformation("Organizations successfully retrieved");
        return organizations.Select(organization => new OrganizationDto
        {
            Id = organization.Id,
            Members = organization.Members.Select(m => new UserDto { Email = m.Email, Id = m.Id }).ToList(),
            Name = organization.Name,
            Organizers = organization.Organizers.Select(org => new UserDto { Email = org.Email, Id = org.Id }).ToList()
        }).ToList();
    }

    /// <summary>
    /// Create organization if user's token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="name">Name of organization.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPost("create")]
    [AuthorizationFilter]
    public async Task Create(
        [FromHeader(Name = "Token")] string token,
        [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to create organization");

        var creatorId = HttpExtensions.GetIdFromItems(HttpContext);
        await _organizationService.CreateAsync(name, creatorId, cancellationToken);

        _logger.LogInformation("Organization successfully created");
    }

    /// <summary>
    /// Add member to organization if token hasn't expired yet.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="memberEmail">Email of user you want to add.</param>
    /// <param name="organizationId">ID of target organization.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPut("add-member")]
    [AuthorizationFilter]
    public async Task AddMember(
        [FromHeader(Name = "Token")] string token,
        [FromQuery] Guid organizationId,
        [FromQuery] string memberEmail,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to add member");

        var requesterId = HttpExtensions.GetIdFromItems(HttpContext);
        var organization = await _organizationService.GetByIdAsync(organizationId, cancellationToken);
        if (organization.Members.All(u => u.Id != requesterId) && organization.Organizers.All(u => u.Id != requesterId))
        {
            throw new InvalidRequestException("Only members or organizers can add members");
        }

        await _organizationService.AddMemberAsync(organizationId, memberEmail, cancellationToken);

        _logger.LogInformation("Member successfully added");
    }

    /// <summary>
    /// Add organizer to organization if token hasn't expired yet and requester is organizer.
    /// </summary>
    /// <param name="token">Token that is used to verify the user. Token locates on header "Token".</param>
    /// <param name="organizerEmail">Email of user you want to add.</param>
    /// <param name="organizationId">ID of target organization.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    [HttpPut("add-organizer")]
    [AuthorizationFilter]
    public async Task AddOrganizer(
        [FromHeader(Name = "Token")] string token,
        [FromQuery] Guid organizationId,
        [FromQuery] string organizerEmail,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to add organizer");

        var requesterId = HttpExtensions.GetIdFromItems(HttpContext);
        var organization = await _organizationService.GetByIdAsync(organizationId, cancellationToken);
        if (organization.Organizers.All(u => u.Id != requesterId))
        {
            throw new InvalidRequestException("Only organizers can add organizers");
        }

        await _organizationService.AddOrganizerAsync(organizationId, organizerEmail, cancellationToken);

        _logger.LogInformation("Organizer successfully added");
    }
}