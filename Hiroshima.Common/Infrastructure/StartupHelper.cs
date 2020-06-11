using System;
using HappyTravel.VaultClient;
using Microsoft.Extensions.Configuration;

namespace Hiroshima.Common.Infrastructure
{
    public class StartupHelper
    {
        public static VaultClient CreateVaultClient(IConfiguration configuration)
        {
            var vaultOptions = new VaultOptions
            {
                BaseUrl = new Uri(configuration[configuration["Vault:Endpoint"]]),
                Engine = configuration["Vault:Engine"],
                Role = configuration["Vault:Role"]
            };
            
            return new VaultClient(vaultOptions);
        }
        
        
        public static string GetDbConnectionString(VaultClient vaultClient, string pathInAppSettings, IConfiguration configuration)
        {
            var connectionOptions = vaultClient.Get(configuration[$"{pathInAppSettings}:ConnectionOptions"]).Result;
            
            return string.Format($"{configuration[$"{pathInAppSettings}:ConnectionString"]}",
                connectionOptions["host"],
                connectionOptions["port"],
                connectionOptions["database"],
                connectionOptions["userId"],
                connectionOptions["password"]);
        }
        
        
        public static string GetAspNetcoreEnvironment()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(env))
                throw new Exception("ASPNETCORE_ENVIRONMENT variable isn't defined");
            return env;
        }
    }
}