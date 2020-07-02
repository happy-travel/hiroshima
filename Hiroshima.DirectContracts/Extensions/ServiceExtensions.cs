using System;
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
        public static IServiceCollection AddDirectContractsServices(this IServiceCollection services, DcOptions dcOptions)
        {
            if (dcOptions.Equals(default))
                throw new ArgumentNullException($"{nameof(dcOptions)} is null");
            
            services.AddEntityFrameworkNpgsql().AddDbContextPool<DirectContractsDbContext>(options =>
            {
                options.UseNpgsql(dcOptions.ConnectionString, npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure();
                        npgsqlOptions.UseNetTopologySuite();
                    });
                options.EnableSensitiveDataLogging(false);

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, 16);

            services.AddTransient<IAvailabilityService, AvailabilityService>();
            services.AddTransient<IRoomAvailabilityService, RoomAvailabilityService>();
            services.AddTransient<IRateAvailabilityService, RateAvailabilityService>();
            services.AddTransient<ICancellationPoliciesService, CancellationPoliciesService>();
            services.AddSingleton<IPaymentDetailsService, PaymentDetailsService>();
            services.AddSingleton<ICancellationPoliciesService, CancellationPoliciesService>();
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

            return services;
        }
    }
}