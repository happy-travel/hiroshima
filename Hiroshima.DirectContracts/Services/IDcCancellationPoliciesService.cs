using System;
using System.Collections.Generic;
using Hiroshima.DbData.Models.Rooms.CancellationPolicies;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDcCancellationPoliciesService
    {
        List<CancellationPolicyDetails> Get(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, PaymentDetails paymentDetails);
    }
}