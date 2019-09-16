using Hiroshima.DbData;
using Hiroshima.DirectContracts.Infrastructure.Options;
using Hiroshima.DirectContracts.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hiroshima.DirectContracts
{
    public class DirectContracts : IDirectContracts
    {
        public DirectContracts(IOptions<InitOptions> initOptions)
        {
            var dbOptions = initOptions.Value.DatabaseOptions;
            _host = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddEntityFrameworkNpgsql().AddDbContextPool<DcDbContext>(options =>
                    {
                        options.UseNpgsql($"server={dbOptions.Host};" +
                                          $"port={dbOptions.Port};" +
                                          $"database={dbOptions.Database};" +
                                          $"userId={dbOptions.UserId};" +
                                          $"password={dbOptions.Password};", npgsqlOptions =>
                        {
                            npgsqlOptions.EnableRetryOnFailure();
                            npgsqlOptions.UseNetTopologySuite();
                        });
                        options.EnableSensitiveDataLogging(false);

                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    }, 16);
                services.AddTransient<IAvailabilitySearch, AvailabilitySearch>();
                services.AddTransient<ILocations, Locations>();

                services.TryAddSingleton(initOptions);
            }).ConfigureLogging(logging => logging.AddConsole()).Build();
        }

        public IAvailabilitySearch AvailabilitySearch => _host.Services.GetService<IAvailabilitySearch>();
        
        public ILocations Locations => _host.Services.GetService<ILocations>();

        private readonly IHost _host;
    }

    
}
