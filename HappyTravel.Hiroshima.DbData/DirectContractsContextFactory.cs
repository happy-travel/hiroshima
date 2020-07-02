using System.IO;
using System.Reflection;
using HappyTravel.Hiroshima.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HappyTravel.Hiroshima.DbData
{
    class DirectContractsContextFactory : IDesignTimeDbContextFactory<DirectContractsDbContext>
    {
        public DirectContractsDbContext CreateDbContext(string[] args)
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
            var connectionString = StartupHelper.GetDbConnectionString(vaultClient, "Database",  configuration);
            
            var dbContextOptions = new DbContextOptionsBuilder<DirectContractsDbContext>();
            dbContextOptions.UseNpgsql(connectionString, 
                builder => builder.UseNetTopologySuite());

            return new DirectContractsDbContext(dbContextOptions.Options);
        }
    }
}