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
            
            services.AddEntityFrameworkNpgsql().AddDbContextPool<DcDbContext>(options =>
            {
                options.UseNpgsql(dcOptions.ConnectionString, npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure();
                        npgsqlOptions.UseNetTopologySuite();
                    });
                options.EnableSensitiveDataLogging(false);

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, 16);

            services.AddTransient<IDcAvailabilityService, DcAvailabilityService>();
            services.AddTransient<IDcRoomAvailabilityService, DcRoomAvailabilityService>();
            services.AddTransient<IDcAccommodationAvailabilityService, DcAccommodationAvailabilityService>();
            services.AddTransient<IDcRateAvailabilityService, DcRateAvailabilityService>();
            services.AddTransient<IDcCancellationPoliciesService, DcCancellationPoliciesService>();
            services.AddSingleton<IDcAvailableRatePaymentService, DcAvailableRatePaymentService>();
            services.AddSingleton<IDcCancellationPoliciesService, DcCancellationPoliciesService>();

            return services;
        }
    }
}