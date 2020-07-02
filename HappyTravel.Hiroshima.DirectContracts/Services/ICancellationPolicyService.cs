using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.DbData.Models.Room.CancellationPolicies;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public interface ICancellationPolicyService
    {
        List<CancellationPolicyDetails> Get(RoomCancellationPolicy roomCancellationPolicy, DateTime checkInDate, PaymentDetails paymentDetails);
    }
}