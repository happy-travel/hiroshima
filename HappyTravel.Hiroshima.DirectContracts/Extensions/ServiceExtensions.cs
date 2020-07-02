using System;
using HappyTravel.Hiroshima.DbData;
using HappyTravel.Hiroshima.DirectContracts.Infrastructure.Options;
using HappyTravel.Hiroshima.DirectContracts.Services;
using HappyTravel.Hiroshima.DirectContracts.Services.Availability;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.Hiroshima.DirectContracts.Extensions
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
            services.AddTransient<ICancellationPolicyService, CancellationPolicyService>();
            services.AddSingleton<IPaymentDetailsService, PaymentDetailsService>();
            services.AddSingleton<ICancellationPolicyService, CancellationPolicyService>();
            services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();

            return services;
        }
    }
}