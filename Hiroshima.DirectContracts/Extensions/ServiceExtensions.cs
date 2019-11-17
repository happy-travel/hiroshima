using Hiroshima.DbData;
using Hiroshima.DirectContracts.Infrastructure.Options;
using Hiroshima.DirectContracts.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hiroshima.DirectContracts.Extensions
{
    public static class ServiceExtensions
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

            services.AddTransient<IDirectContractsAvailability, DirectContractsAvailability>();
            services.AddTransient<IDirectContractsLocation, DirectContractsLocation>();
            services.AddTransient<IDirectContractsDatabaseRequests, DirectContractsDatabaseRequests>();
            services.AddTransient<IDirectContractsAvailabilityResponse, DirectContractsAvailabilityResponse>();
            services.AddTransient<IDirectContractsRawDataFilter, DirectContractsRawDataFilter>();
            services.AddTransient<IDirectContractsPrices, DirectContractsPrices>();
            services.AddTransient<IDirectContractsCancelationPolicies, DirectContractsCancelationPolicies>();

            return services;
        }
    }
}