using System;
using System.Reflection.Metadata.Ecma335;
using Amazon;
using HappyTravel.AmazonS3Client.Options;
using HappyTravel.VaultClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HappyTravel.Hiroshima.Common.Infrastructure
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
        public static string GetDbConnectionString(VaultClient.VaultClient vaultClient, string pathToConnectionOptions, string pathToConnectionString, IConfiguration configuration)
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
        /// The method to get a Amazon S3 credentials for the contract documents from the Vault
        /// </summary>
        /// <param name="vaultClient">The instance of the Vault client </param>
        /// <param name="pathToConnectionOptions">The path to connection options in appsettings.json</param>
        /// <param name="pathToConnectionString">The path to the connection string template in appsettings.json</param>
        /// <param name="configuration">Represents the application configuration</param>
        /// <returns></returns>
        public static AmazonS3ClientOptions GetAmazonS3Credentials(VaultClient.VaultClient vaultClient, string pathToAmazonS3Credentials, IConfiguration configuration)
        {
            var amazonS3Credentials = vaultClient.Get(configuration[pathToAmazonS3Credentials]).Result;

            return new AmazonS3ClientOptions
            {
                AmazonS3Config = new Amazon.S3.AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(amazonS3Credentials["regionEndpoint"])
                },
                AccessKeyId = amazonS3Credentials["accessKeyId"],
                AccessKey = amazonS3Credentials["accessKey"]
            };
        }
    }
}