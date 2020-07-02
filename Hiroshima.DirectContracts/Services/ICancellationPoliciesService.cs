using System;
using System.Collections.Generic;
using Hiroshima.DbData.Models.Room.CancellationPolicies;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services
{
    public interface ICancellationPoliciesService
    {
        List<CancellationPolicyDetails> Get(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, PaymentDetails paymentDetails);
    }
}