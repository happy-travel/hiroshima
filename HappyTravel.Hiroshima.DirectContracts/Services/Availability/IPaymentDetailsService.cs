using System;
using System.Collections.Generic;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;
using HappyTravel.Hiroshima.Common.Models.Availabilities;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public interface IPaymentDetailsService
    {
        PaymentDetails Create(DateTime checkInDate, DateTime checkOutDate,
            List<RoomRate> rateDetails, List<RoomPromotionalOffer> roomPromotionalOffers, string languageCode);
    }
}