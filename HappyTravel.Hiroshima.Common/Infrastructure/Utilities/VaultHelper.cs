using System;
using System.Collections.Generic;
using HappyTravel.VaultClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class VaultHelper
    {
        public static VaultClient.VaultClient CreateVaultClient(IConfiguration configuration, ILoggerFactory loggerFactory = null)
        {
            var vaultOptions = new VaultOptions
            {
                BaseUrl = new Uri(configuration[configuration["Vault:Endpoint"]]),
                Engine = configuration["Vault:Engine"],
                Role = configuration["Vault:Role"]
            };

            return new VaultClient.VaultClient(vaultOptions, loggerFactory);
        }
        
        
        /// <summary>
        /// The method to get a DB connection string from the Vault using options from appsettings.json
        /// </summary>
        /// <param name="vaultClient">The instance of the Vault client </param>
        /// <param name="pathToConnectionOptions">The path to connection options in appsettings.json</param>
        /// <param name="pathToConnectionString">The path to the connection string template in appsettings.json</param>
        /// <param name="configuration">Represents the application configuration</param>
        /// <returns></returns>
        public static string GetDbConnectionString(IVaultClient vaultClient, string pathToConnectionOptions, string pathToConnectionString, IConfiguration configuration)
        {
            var connectionOptions = vaultClient.Get(configuration[pathToConnectionOptions]).Result;
            return string.Format($"{configuration[pathToConnectionString]}",
                connectionOptions["host"],
                connectionOptions["port"],
                connectionOptions["database"],
                connectionOptions["userId"],
                connectionOptions["password"]);
        }


        /// <summary>
        /// The method to get secret strings from the Vault
        /// </summary>
        /// <param name="vaultClient">The instance of the Vault client </param>
        /// <param name="path">The path to the secret name in appsettings.json</param>
        /// <param name="configuration">Represents the application configuration</param>
        public static Dictionary<string, string> GetOptions(IVaultClient vaultClient, string path, IConfiguration configuration) 
            => vaultClient.Get(configuration[path]).Result;
    }
}