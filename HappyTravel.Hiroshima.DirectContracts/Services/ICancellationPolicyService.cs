using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public interface ICancellationPolicyService
    {
        List<CancellationPolicyDetails> Create(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, PaymentDetails paymentDetails);
    }
}