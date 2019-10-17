using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HappyTravel.VaultClient;
using HappyTravel.VaultClient.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hiroshima.DbData
{
    class DirectContractsContextFactory : IDesignTimeDbContextFactory<DirectContractsDbContext>
    {
        public DirectContractsDbContext CreateDbContext(string[] args)
        {
            var dbContextOptions = new DbContextOptionsBuilder<DirectContractsDbContext>();
            dbContextOptions.UseNpgsql(GetConnectionString(), builder => builder.UseNetTopologySuite());
            return new DirectContractsDbContext(dbContextOptions.Options);
        }


        private static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var dbOptions = GetDbOptions(configuration);
            return $"server={dbOptions["host"]};port={dbOptions["port"]};database={dbOptions["database"]};userid={dbOptions["userId"]};password={dbOptions["password"]};";
        }


        private static Dictionary<string, string> GetDbOptions(IConfiguration configuration)
        {
            using (var vaultClient = CreateVaultClient(configuration))
            {
                vaultClient.Login(Environment.GetEnvironmentVariable(configuration["Vault:Token"])).Wait();
                return vaultClient.Get(configuration["DirectContracts:Database:Options"]).Result;
            }
        }


        private static IVaultClient CreateVaultClient(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.AddVaultClient(o =>
            {
                o.Engine = configuration["Vault:Engine"];
                o.Role = configuration["Vault:Role"];
                o.Url = new Uri(configuration["Vault:Endpoint"]);
            });
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<IVaultClient>();
        }
    }
}