using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Availabilities;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public interface ICancellationPolicyService
    {
        List<CancellationPolicyDetails> Create(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, DateTime checkoutDate, PaymentDetails paymentDetails);
    }
}