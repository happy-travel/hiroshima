using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Hiroshima.WebApi.Infrastructure
{
    public class ControllerResolveHealthcheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            //todo implement logic
            return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy));
        }
    }
}
