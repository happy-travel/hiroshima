using System;
using System.Globalization;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.VaultClient;
using Hiroshima.DirectContracts;
using Hiroshima.DirectContracts.Infrastructure.Options;
using Hiroshima.WebApi.Infrastructure;
using Hiroshima.WebApi.Infrastructure.Constants;
using Hiroshima.WebApi.Services;
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

namespace Hiroshima.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = false;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
            
            DbOptions bookingDbOptions;
            using (var vaultClient = new VaultClient(new VaultOptions
            {
                Engine = Configuration["Vault:Engine"],
                Role = Configuration["Vault:Role"],
                BaseUrl = new Uri(Configuration["Vault:Endpoint"])
            }, null))
            {
                vaultClient.Login(Configuration[Configuration["Vault:Token"]]).GetAwaiter().GetResult();
                var dbOptions = vaultClient.Get(Configuration["DirectContracts:Database:Options"]).GetAwaiter().GetResult();
                bookingDbOptions = new DbOptions(
                    dbOptions["host"],
                    int.Parse(dbOptions["port"]),
                    dbOptions["database"],
                    dbOptions["userId"],
                    dbOptions["password"]);
            }
            
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
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                    })
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
                        new CultureInfo("cn"),
                        new CultureInfo("en"),
                        new CultureInfo("es"),
                        new CultureInfo("fr"),
                        new CultureInfo("ru")
                    };
                    options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
                });

            services.AddDirectContractsServices(bookingDbOptions);
            services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(ConstantValues.Srid));
            services.AddTransient<IAvailability, Availability>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<RequestLocalizationOptions> localizationOptions)
        {
            app.UseRouting();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader());
            app.UseResponseCompression();
            app.UseRequestLocalization(localizationOptions.Value);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        public IConfiguration Configuration { get; }
    }
}
