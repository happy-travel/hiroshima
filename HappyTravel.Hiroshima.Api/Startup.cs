using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Amazon;
using Amazon.S3;
using CacheFlow.Json.Extensions;
using FloxDc.CacheFlow.Extensions;
using FluentValidation.AspNetCore;
using HappyTravel.AmazonS3Client.Extensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.DirectContracts.Extensions;
using HappyTravel.Hiroshima.DirectManager.Extensions;
using HappyTravel.Hiroshima.DirectManager.Infrastructure.Options;
using HappyTravel.Hiroshima.DirectManager.Services;
using HappyTravel.Hiroshima.Api.Conventions;
using HappyTravel.Hiroshima.Api.Filters;
using HappyTravel.Hiroshima.Api.Infrastructure;
using HappyTravel.Hiroshima.Api.Infrastructure.Extensions;
using HappyTravel.MailSender.Infrastructure;
using HappyTravel.MailSender.Models;
using HappyTravel.StdOutLogger.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace HappyTravel.Hiroshima.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetJsonOptions();
            
            using var vaultClient = VaultHelper.CreateVaultClient(Configuration);
            vaultClient.Login(Configuration[Configuration["Vault:Token"]]).GetAwaiter().GetResult();
            var dbConnectionString = VaultHelper.GetDbConnectionString(vaultClient, "DirectContracts:Database:ConnectionOptions", "DirectContracts:Database:ConnectionString", Configuration);
            var redisEndpoint = Configuration[Configuration["Redis:Endpoint"]];
            var amazonS3DocumentsOptions = VaultHelper.GetOptions(vaultClient, "DirectContracts:AmazonS3:Documents", Configuration);
            var amazonS3ImagesOptions = VaultHelper.GetOptions(vaultClient, "DirectContracts:AmazonS3:Images", Configuration);
            var mailSenderOptions = VaultHelper.GetOptions(vaultClient, "DirectContracts:Email", Configuration);
            var bookingWebhookOptions = VaultHelper.GetOptions(vaultClient, "DirectContracts:BookingWebhookOptions", Configuration);

            services.AddDirectContractsServices(dbConnectionString);
            services.AddDirectManagerServices();
            services.AddAmazonS3Client(options => 
            {
                options.AccessKeyId = amazonS3DocumentsOptions["accessKeyId"];
                options.AccessKey = amazonS3DocumentsOptions["accessKey"];
                options.AmazonS3Config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(amazonS3DocumentsOptions["regionEndpoint"])
                };
            });
          
            services.AddLocalization();
            services.AddTracing(HostingEnvironment, Configuration);

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
                    options.AmazonS3Bucket = amazonS3DocumentsOptions["bucket"];
                })
                .Configure<ImageManagementServiceOptions>(options =>
                 {
                     options.AmazonS3Bucket = amazonS3ImagesOptions["bucket"];
                     options.AmazonS3RegionEndpoint = amazonS3ImagesOptions["regionEndpoint"];
                 })
                .Configure<SenderOptions>(options =>
                {
                    options.ApiKey = mailSenderOptions["apiKey"];
                    options.BaseUrl = new Uri(mailSenderOptions["publicUrl"]);
                    options.SenderAddress = new EmailAddress(mailSenderOptions["senderAddress"]);
                })
                .Configure<NotificationServiceOptions>(options =>
                {
                    options.ManagerInvitationMessageTemplateId = mailSenderOptions["regularManagerInvitationTemplateId"];
                    options.ManagerRegistrationMessageTemplateId = mailSenderOptions["regularManagerRegistrationTemplateId"];
                    options.NewMasterManagerWelcomeMessageTemplateId = mailSenderOptions["newMasterManagerWelcomeTemplateId"];
                })
                .Configure<BookingWebhookOptions>(options =>
                {
                    options.Key = bookingWebhookOptions["key"];
                    options.WebhookUrl = new Uri(bookingWebhookOptions["webhookUrl"]);
                })
                .Configure<ManagerInvitationOptions>(options =>
                {
                    options.InvitationExpirationPeriod = TimeSpan.FromDays(7);
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
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        Array.Empty<string>()
                    }
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IOptions<RequestLocalizationOptions> localizationOptions)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRequestLocalization(localizationOptions.Value);

            app.UseHttpContextLogging(
                options => options.IgnoredPaths = new HashSet<string> { "/health" }
            );

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
        public IWebHostEnvironment HostingEnvironment { get; }
    }
}