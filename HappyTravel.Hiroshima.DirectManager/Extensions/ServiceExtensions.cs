using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Services;
using LocationNameNormalizer.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.Hiroshima.DirectManager.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDirectManagerServices(this IServiceCollection services)
        {
            services.AddTransient<IAccommodationManagementService, AccommodationManagementService>();
            services.AddTransient<IImageManagementService, ImageManagementService>();
            services.AddTransient<IContractManagementService, ContractManagementService>();
            services.AddTransient<IDocumentManagementService, DocumentManagementService>();
            services.AddTransient<ISeasonManagementService, SeasonManagementService>();
            services.AddTransient<IRateManagementService, RateManagementService>();
            services.AddTransient<IPromotionalOfferManagementService, PromotionalOfferManagementService>();
            services.AddTransient<ICancellationPolicyManagementService, CancellationPolicyManagementService>();
            services.AddTransient<ILocationManagementService, LocationManagementService>();
            services.AddTransient<IAllocationRequirementManagementService, AllocationRequirementManagementService>();
            services.AddTransient<IAvailabilityRestrictionsManagementService, AvailabilityRestrictionsManagementService>();
            services.AddTransient<IAmenityService, AmenityService>();
            services.AddScoped<IManagerContextService, ManagerContextService>();
            services.AddTransient<IManagerManagementService, ManagerManagementService>();
            services.AddScoped<ITokenInfoAccessor, TokenInfoAccessor>();
            services.AddNameNormalizationServices();
            
            return services;
        }
    }
}