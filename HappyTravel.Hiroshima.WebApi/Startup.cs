using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Amazon.S3;
using CacheFlow.Json.Extensions;
using FloxDc.CacheFlow.Extensions;
using FluentValidation.AspNetCore;
using HappyTravel.AmazonS3Client.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectContracts.Extensions;
using HappyTravel.Hiroshima.DirectManager.Extensions;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.WebApi.Conventions;
using HappyTravel.Hiroshima.WebApi.Filters;
using HappyTravel.Hiroshima.WebApi.Infrastructure;
using HappyTravel.Hiroshima.WebApi.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HappyTravel.Hiroshima.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            using var vaultClient = VaultHelper.CreateVaultClient(Configuration);
            vaultClient.Login(Configuration[Configuration["Vault:Token"]]).GetAwaiter().GetResult();
            var dbConnectionString = VaultHelper.GetDbConnectionString(vaultClient, "DirectContracts:Database:ConnectionOptions", "DirectContracts:Database:ConnectionString", Configuration);
            var redisEndpoint = Configuration[Configuration["Redis:Endpoint"]];
            var amazonS3ClientOptions = VaultHelper.GetAmazonS3Credentials(vaultClient, "DirectContracts:AmazonS3:Contracts", Configuration);
            var amazonS3Bucket = VaultHelper.GetAmazonS3BucketName(vaultClient, "DirectContracts:AmazonS3:Contracts", Configuration);
            var amazonS3RegionEndpoint = VaultHelper.GetAmazonS3RegionEndpoint(vaultClient, "DirectContracts:AmazonS3:Contracts", Configuration);

            services.AddDirectContractsServices(dbConnectionString);
            services.AddDirectManagerServices();
            services.AddAmazonS3Client(options => 
            { 
                options.AccessKey = amazonS3ClientOptions.AccessKey;
                options.AccessKeyId = amazonS3ClientOptions.AccessKeyId;
                options.AmazonS3Config = new AmazonS3Config
                { 
                    RegionEndpoint = amazonS3ClientOptions.AmazonS3Config.RegionEndpoint 
                };
            });
          
            services.AddLocalization();
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
                })
                .Configure<DocumentManagementServiceOptions>(options => 
                {
                    options.AmazonS3Bucket = amazonS3Bucket;
                })
                .Configure<ImageManagementServiceOptions>(options =>
                 {
                     options.AmazonS3Bucket = amazonS3Bucket;
                     options.AmazonS3RegionEndpoint = amazonS3RegionEndpoint;
                 });

            services.AddHealthChecks()
                .AddCheck<ControllerResolveHealthCheck>(nameof(ControllerResolveHealthCheck))
                .AddDbContextCheck<DirectContractsDbContext>()
                .AddRedis(redisEndpoint);
                
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            
            services.AddResponseCompression()
                .AddHttpContextAccessor()
                .AddCors()
                .AddLocalization()
                .AddMemoryCache()
                .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisEndpoint;
                })
                .AddDoubleFlow()
                .AddCacheFlowJsonSerialization();
            services.AddHttpContextAccessor();
            services.AddMvcCore(options => 
            {
                options.Conventions.Insert(0, new LocalizationConvention());
                options.Conventions.Add(new AuthorizeControllerModelConvention());
                options.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipelineFilter)));
                    options.Filters.Add(typeof(ModelValidationFilter));
            })
            .AddAuthorization()
            .AddControllersAsServices()
            .AddFormatterMappings()
            .AddApiExplorer()
            .AddFluentValidation()
            .SetCompatibilityVersion(CompatibilityVersion.Latest);
        
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters = new List<JsonConverter>
                {
                    new Newtonsoft.Json.Converters.StringEnumConverter()
                };
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            
            services.ConfigureAuthentication(Configuration, HostingEnvironment, vaultClient)
                .ConfigureHttpClients(Configuration, HostingEnvironment, vaultClient)
                .AddServices();
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo {Title = "HappyTravel.com Direct Contracts API", Version = "v1.0"});

                var xmlCommentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
                options.CustomSchemaIds(t => t.FullName);
                options.IncludeXmlComments(xmlCommentsFilePath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptions<RequestLocalizationOptions> localizationOptions)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRequestLocalization(localizationOptions.Value);
            
            app.UseResponseCompression();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
           
            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "HappyTravel.com Emerging Travel Group Connector API");
                    options.RoutePrefix = string.Empty;
                });
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
        
        
        public IConfiguration Configuration { get; }
        public IHostEnvironment HostingEnvironment { get; }
    }
}