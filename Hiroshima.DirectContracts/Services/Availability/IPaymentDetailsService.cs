using System;
using System.Collections.Generic;
using Hiroshima.DbData.Models.Room;
using Hiroshima.DirectContracts.Models;

namespace Hiroshima.DirectContracts.Services.Availability
{
    public interface IPaymentDetailsService
    {
        PaymentDetails Create(DateTime checkInDate, DateTime checkOutDate,
            List<RoomRate> roomRates, RoomPromotionalOffer roomPromotionalOffer = null);
    }
}