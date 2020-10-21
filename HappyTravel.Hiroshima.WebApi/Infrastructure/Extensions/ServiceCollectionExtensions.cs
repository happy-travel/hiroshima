using System;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.WebApi.Infrastructure.Environments;
using HappyTravel.Hiroshima.WebApi.Services;
using HappyTravel.VaultClient;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetTopologySuite;

namespace HappyTravel.Hiroshima.WebApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment environment, IVaultClient vaultClient)
        {
            var (apiName, authorityUrl) = GetApiNameAndAuthority(configuration, environment, vaultClient);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authorityUrl;
                    options.ApiName = apiName;
                    options.RequireHttpsMetadata = true;
                    options.SupportedTokens = SupportedTokens.Jwt;
                });

            return services;
        }


        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment,
            IVaultClient vaultClient)
        {
            var (_, authorityUrl) = GetApiNameAndAuthority(configuration, environment, vaultClient);
            services.AddHttpClient<IdentityHttpClient>(client => client.BaseAddress = new Uri(authorityUrl));

            return services;
        }


        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(GeoConstants.SpatialReferenceId));
            services.AddTransient<IAvailabilityService, AvailabilityService>();
            
            return services;
        }

        
        private static (string apiName, string authorityUrl) GetApiNameAndAuthority(IConfiguration configuration, IWebHostEnvironment environment,
            IVaultClient vaultClient)
        {
            var authorityOptions = vaultClient.Get(configuration["Authority:Options"]).GetAwaiter().GetResult();

            var apiName = configuration["Authority:ApiName"];
            var authorityUrl = configuration["Authority:Endpoint"];
            if (environment.IsDevelopment() || environment.IsLocal())
                return (apiName,  authorityUrl);

            apiName = authorityOptions["apiName"];
            authorityUrl = authorityOptions["authorityUrl"];

            return (apiName, authorityUrl);
        }
    }
}