using System;
using System.Globalization;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.VaultClient;
using HappyTravel.VaultClient.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hiroshima.DirectContracts;
using Hiroshima.DirectContracts.Infrastructure.Options;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTopologySuite;
using Newtonsoft.Json;
using WebApi.Infrastructure;
using WebApi.Services;

namespace WebApi
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

           services.AddVaultClient(options =>
            {
                options.Engine = Configuration["Vault:Engine"];
                options.Role = Configuration["Vault:Role"];
                options.Url = new Uri(Configuration["Vault:Endpoint"]);
            });

            DbOptions bookingDbOptions;

            //TODO need to fix
            var serviceProvider = services.BuildServiceProvider();
            using (var vaultClient = serviceProvider.GetService<IVaultClient>())
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

            services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(4326));

            services.AddDirectContractsService(options =>
            {
                options.DatabaseOptions = bookingDbOptions;
            });

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

            services.AddHealthChecks().
                AddCheck<ControllerResolveHealthcheck>(nameof(ControllerResolveHealthcheck));

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
            
            services.AddTransient<IDcRequestConverter, DcRequestConverter>();
            services.AddTransient<IDcResponseConverter, DcResponseConverter>();
            services.AddTransient<IAvailabilityService, AvailabilityService>();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<RequestLocalizationOptions> localizationOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyHeader());
            app.UseResponseCompression();
           // app.UseHttpsRedirection();

            app.UseRequestLocalization(localizationOptions.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
