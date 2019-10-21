using Hiroshima.DbData;
using Hiroshima.DirectContracts.Infrastructure.Options;
using Hiroshima.DirectContracts.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hiroshima.DirectContracts
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDirectContractsServices(this IServiceCollection services, DbOptions dbOptions)
        {
            services.AddEntityFrameworkNpgsql().AddDbContextPool<DirectContractsDbContext>(options =>
            {
                options.UseNpgsql($"server={dbOptions.Host};" +
                                  $"port={dbOptions.Port};" +
                                  $"database={dbOptions.Database};" +
                                  $"userId={dbOptions.UserId};" +
                                  $"password={dbOptions.Password};", npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure();
                    npgsqlOptions.UseNetTopologySuite();
                });
                options.EnableSensitiveDataLogging(false);

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, 16);
            services.AddTransient<IDcAvailability, DcAvailability>();
            services.AddTransient<IDcLocations, DcLocations>();
            return services;
        }

    }
}
