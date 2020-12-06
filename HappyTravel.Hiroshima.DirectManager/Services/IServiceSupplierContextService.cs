using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IServiceSupplierContextService
    {
        Result<ServiceSupplier> EnsureContractBelongsToServiceSupplier(ServiceSupplier serviceSupplier, int contractId);
    }
}
