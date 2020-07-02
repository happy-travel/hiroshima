using System.Globalization;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.Geography;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.DirectContracts.Extensions;
using HappyTravel.Hiroshima.DirectContracts.Infrastructure.Options;
using HappyTravel.Hiroshima.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetTopologySuite;
using Newtonsoft.Json;

namespace HappyTravel.Hiroshima.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = false;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            using var vaultClient = StartupHelper.CreateVaultClient(Configuration);
            vaultClient.Login(Configuration[Configuration["Vault:Token"]]).GetAwaiter().GetResult();
            var dbConnectionString = StartupHelper.GetDbConnectionString(vaultClient, "DirectContracts:Database", Configuration);
           
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddMvcCore()
                .AddControllersAsServices()
                .AddFormatterMappings()
                .AddApiExplorer()
                .AddNewtonsoftJson(options => { options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors()
                .AddLocalization()
                .AddMemoryFlow()
                .AddMemoryCache();

            services.AddResponseCompression();
            services.AddHealthChecks();
            services.AddOptions()
                .Configure<RequestLocalizationOptions>(options =>
                {
                    options.DefaultRequestCulture = new RequestCulture("en");
                    options.SupportedCultures = new[]
                    {
                        new CultureInfo("ar"),
                        new CultureInfo("en"),
                        new CultureInfo("ru")
                    };
                    options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider {Options = options});
                });

            services.AddDirectContractsServices(new DcOptions{ConnectionString = dbConnectionString});
            services.AddSingleton(
                NtsGeometryServices.Instance.CreateGeometryFactory(
                    GeoConstants.SpatialReferenceId));
            services.AddTransient<IAvailabilityService, AvailabilityService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<RequestLocalizationOptions> localizationOptions)
        {
            app.UseRequestLocalization(localizationOptions.Value);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader());
            app.UseResponseCompression();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}