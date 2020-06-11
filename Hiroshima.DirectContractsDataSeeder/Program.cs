using System;
using System.IO;
using System.Reflection;
using Hiroshima.Common.Infrastructure;
using Hiroshima.DbData;
using Hiroshima.DbData.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hiroshima.DirectContractsDataSeeder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var dbContext = CreateDbContext();
            DataSeeder.AddData(dbContext);
            Console.WriteLine("Seeding is complete");
        }


        private static DirectContractsDbContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", 
                    false,
                    true)
                .AddEnvironmentVariables()
                .Build();
            
            var dbContextOptions = new DbContextOptionsBuilder<DirectContractsDbContext>();
            using var vaultClient = StartupHelper.CreateVaultClient(configuration);
            vaultClient.Login(configuration[configuration["Vault:Token"]]).GetAwaiter().GetResult();
            //var connectionString = StartupHelper.GetDbConnectionString(vaultClient, "Database:ConnectionOptions", configuration);
            var connectionString = "Server=localhost;Port=5433;Database=directcontracts;Userid=postgres;Password=postgress";
            dbContextOptions.UseNpgsql(connectionString, builder => builder.UseNetTopologySuite());
            return new DirectContractsDbContext(dbContextOptions.Options);
        }
    }
}
