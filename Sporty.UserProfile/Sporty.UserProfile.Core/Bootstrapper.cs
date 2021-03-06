using Microsoft.Extensions.DependencyInjection;
using Sporty.UserProfile.Core.Organizations.Services;
using Sporty.UserProfile.Core.Organizations.Services.Interfaces;
using Sporty.UserProfile.Core.Users.Services;
using Sporty.UserProfile.Core.Users.Services.Interfaces;
using Sporty.UserProfile.Domain.Encoders;

namespace Sporty.UserProfile.Core;

public static class Bootstrapper
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IPasswordEncoder, BCryptPasswordEncoder>();

        return services;
    }
}