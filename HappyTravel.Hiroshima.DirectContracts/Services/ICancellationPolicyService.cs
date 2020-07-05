using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Data.Models.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public interface ICancellationPolicyService
    {
        List<CancellationPolicyDetails> Get(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, PaymentDetails paymentDetails);
    }
}