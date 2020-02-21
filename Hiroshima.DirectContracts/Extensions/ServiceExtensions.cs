using Hiroshima.DbData;
using Hiroshima.DirectContracts.Infrastructure.Options;
using Hiroshima.DirectContracts.Services;
using Hiroshima.DirectContracts.Services.Availability;
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

            services.AddTransient<IDirectContractsAvailabilityService, DirectContractsAvailabilityService>();
            services.AddTransient<IDirectContractsLocationService, DirectContractsLocationService>();
            services.AddTransient<IAvailabilityQueriesService, AvailabilityQueriesService>();
            services.AddTransient<IAvailabilityResponseService, AvailabilityResponseService>();
            services.AddTransient<IRawAvailabilityDataFilter, RawAvailabilityDataFilter>();
            services.AddTransient<IPricesService, PricesService>();
            services.AddTransient<ICancelationPoliciesService, CancelationPoliciesService>();

            return services;
        }
    }
}