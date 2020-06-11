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
            if (dcOptions == null)
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

            services.AddTransient<IDirectContractsAvailabilityService, DirectContractsAvailabilityService>();
            services.AddTransient<IDirectContractsRoomAvailabilityService, DirectContractsRoomAvailabilityService>();
            services.AddTransient<IDirectContractsAccommodationAvailabilityService, DirectContractsAccommodationAvailabilityService>();
            services.AddTransient<IDirectContractsRateAvailabilityService, DirectContractsRateAvailabilityService>();
            services.AddTransient<IDirectContractsCancellationPoliciesService, DirectContractsCancellationPoliciesService>();
       
            /*services.AddTransient<IDirectContractsLocationService, DirectContractsLocationService>();
            services.AddTransient<IAvailabilityQueriesService, AvailabilityQueriesService>();
            services.AddTransient<IAvailabilityResponseService, AvailabilityResponseService>();
            services.AddTransient<IRawAvailabilityDataFilter, RawAvailabilityDataFilter>();
            services.AddTransient<IPricesService, PricesService>();
            services.AddTransient<ICancelationPoliciesService, CancelationPoliciesService>();*/

            return services;
        }
    }
}