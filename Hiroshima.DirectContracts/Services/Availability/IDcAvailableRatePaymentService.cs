using System;
using System.Collections.Generic;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IDcAvailableRatePaymentService
    {
        PaymentDetails CreatePaymentDetails(DateTime checkInDate, DateTime checkOutDate,
            List<RoomRate> roomRates, RoomPromotionalOffer roomPromotionalOffer = null);
    }
}