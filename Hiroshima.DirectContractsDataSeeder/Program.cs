﻿using System;
using System.IO;
using System.Reflection;
using Hiroshima.Common.Infrastructure;
using Hiroshima.DbData;
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


        private static DcDbContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", 
                    false,
                    true)
                .AddEnvironmentVariables()
                .Build();
            
            var dbContextOptions = new DbContextOptionsBuilder<DcDbContext>();
            using var vaultClient = StartupHelper.CreateVaultClient(configuration);
            vaultClient.Login(configuration[configuration["Vault:Token"]]).GetAwaiter().GetResult();
            var connectionString = StartupHelper.GetDbConnectionString(vaultClient, "Database:ConnectionOptions", configuration);
            dbContextOptions.UseNpgsql(connectionString, builder => builder.UseNetTopologySuite());
            return new DcDbContext(dbContextOptions.Options);
        }
    }
}
