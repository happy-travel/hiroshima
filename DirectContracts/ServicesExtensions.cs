using System;
using Hiroshima.DirectContracts.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hiroshima.DirectContracts
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDirectContractsService(this IServiceCollection service, Action<InitOptions> initAction)
        {
            service.TryAddSingleton<IDirectContracts, DirectContracts>();
            service.Configure(initAction);
            return service;
        }

    }
}
