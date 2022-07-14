using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sporty.UserProfile.Core.Users.Repositories;
using Sporty.UserProfile.Data.Users.Repositories;
using Microsoft.Extensions.Configuration;

namespace Sporty.UserProfile.Data;

public static class Bootstrapper
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddDbContext<SportyContext>(options => options
            .UseLazyLoadingProxies()
            .UseSqlite(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                configuration["SQLiteConnectionString"])));
        return services;
    }
}