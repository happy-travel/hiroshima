using System;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectContracts.Services;
using HappyTravel.Hiroshima.DirectContracts.Services.Availability;
using HappyTravel.Hiroshima.DirectContracts.Services.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.Hiroshima.DirectContracts.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDirectContractsServices(this IServiceCollection services, string dbConnectionString)
        {
            if (string.IsNullOrEmpty(dbConnectionString))
                throw new ArgumentNullException($"{nameof(dbConnectionString)} is null or empty");
            
            services.AddEntityFrameworkNpgsql().AddDbContextPool<DirectContractsDbContext>(options =>
            {
                options.UseNpgsql(dbConnectionString, npgsqlOptions =>
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
            services.AddTransient<ILocationService, LocationService>();
            services.AddSingleton<IPaymentDetailsService, PaymentDetailsService>();
            services.AddSingleton<ICancellationPolicyService, CancellationPolicyService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IAccommodationManagementRepository, AccommodationManagementRepository>();
            services.AddTransient<IContractManagementRepository, ContractManagementRepository>();
            return services;
        }
    }
}