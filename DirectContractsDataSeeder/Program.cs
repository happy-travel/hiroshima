using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HappyTravel.VaultClient;
using HappyTravel.VaultClient.Extensions;
using Hiroshima.DbData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hiroshima.DirectContractsDataSeeder
{
    class Program
    {
        static void Main(string[] args)
        {
            using var dbContext = CreateDbContext();
            DataSeeder.AddData(dbContext);
        }
        private static DcDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DcDbContext>();
            dbContextOptions.UseNpgsql(GetConnectionString(), builder => builder.UseNetTopologySuite());
            return new DcDbContext(dbContextOptions.Options);
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
                return vaultClient.Get(configuration["Booking:Database:Options"]).Result;
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
