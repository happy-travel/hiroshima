using System.IO;
using System.Reflection;
using Hiroshima.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hiroshima.DbData
{
    class DirectContractsContextFactory : IDesignTimeDbContextFactory<DcDbContext>
    {
        public DcDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", 
                    false,
                    true)
                .AddEnvironmentVariables()
                .Build();

            using var vaultClient = StartupHelper.CreateVaultClient(configuration);
            vaultClient.Login(configuration[configuration["Vault:Token"]]).GetAwaiter().GetResult();
            var connectionString = StartupHelper.GetDbConnectionString(vaultClient, "Database:ConnectionOptions",  configuration);
            
            var dbContextOptions = new DbContextOptionsBuilder<DcDbContext>();
            dbContextOptions.UseNpgsql(connectionString, 
                builder => builder.UseNetTopologySuite());

            return new DcDbContext(dbContextOptions.Options);
        }
    }
}