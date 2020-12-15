using HappyTravel.Hiroshima.DirectManager.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.DirectManager.Services.Bookings;
using HappyTravel.MailSender;
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
            services.AddScoped<IServiceSupplierContextService, ServiceSupplierContextService>();
            services.AddTransient<IManagerManagementService, ManagerManagementService>();
            services.AddTransient<IManagerRegistrationService, ManagerRegistrationService>();
            services.AddTransient<IManagerInvitationService, ManagerInvitationService>();
            services.AddSingleton<IMailSender, SendGridMailSender>();
            services.AddSingleton<ICompanyService, CompanyService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddScoped<ITokenInfoAccessor, TokenInfoAccessor>();
            services.AddTransient<IBookingManagementService, BookingManagementService>();
            services.AddTransient<IBookingWebhookService, BookingWebhookNotificationsService>();
            services.AddNameNormalizationServices();
        
            return services;
        }
    }
}