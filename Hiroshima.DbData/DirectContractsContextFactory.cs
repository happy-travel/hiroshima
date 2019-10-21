using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HappyTravel.VaultClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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
            using (var vaultClient = new VaultClient(new VaultOptions
            {
                Engine = configuration["Vault:Engine"],
                Role = configuration["Vault:Role"],
                BaseUrl = new Uri(configuration["Vault:Endpoint"])
            }, null))
            {
                vaultClient.Login(Environment.GetEnvironmentVariable(configuration["Vault:Token"])).Wait();
                return vaultClient.Get(configuration["DirectContracts:Database:Options"]).Result;
            }
        }
    }
}