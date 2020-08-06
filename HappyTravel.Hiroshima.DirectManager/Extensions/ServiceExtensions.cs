using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HappyTravel.Hiroshima.DirectManager.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDirectManagerServices(this IServiceCollection services)
        {
            services.AddTransient<IAccommodationManagementService, AccommodationManagementService>();
            services.AddTransient<IContractManagementService, ContractManagementService>();
            services.AddTransient<ISeasonManagementService, SeasonManagementService>();
            services.AddTransient<IRateManagementService, RateManagementService>();
            services.AddTransient<IPromotionalOfferManagementService, PromotionalOfferManagementService>();
            services.AddScoped<IContractManagerContextService, ContractManagerContextService>();
            
            return services;
        }
    }
}