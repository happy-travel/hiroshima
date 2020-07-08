using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HappyTravel.Hiroshima.WebApi.Infrastructure
{
    public class ControllerResolveHealthCheck : IHealthCheck
    {
        public ControllerResolveHealthCheck(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            foreach (var controllerType in ControllerTypes)
                _serviceProvider.GetRequiredService(controllerType);

            return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy));
        }


        private static readonly Type[] ControllerTypes = typeof(ControllerResolveHealthCheck).Assembly
            .GetTypes()
            .Where(t =>
            {
                var baseType = t.BaseType;
                while (baseType != null)
                {
                    if (baseType == typeof(ControllerBase))
                        return true;

                    baseType = baseType.BaseType;
                }

                return false;
            })
            .Where(t => !t.IsAbstract && t.IsPublic)
            .ToArray();

        
        private readonly IServiceProvider _serviceProvider;
    }
}