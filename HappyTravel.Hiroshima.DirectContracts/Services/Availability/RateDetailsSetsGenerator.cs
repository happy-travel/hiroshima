using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Internals;
using HappyTravel.Hiroshima.Common.Infrastructure.Utilities;
using HappyTravel.Hiroshima.Common.Models.Availabilities;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class RateDetailsSetsGenerator: IRateDetailsSetsGenerator
    {
        public List<List<RateDetails>> GenerateSets(AvailabilityRequest availabilityRequest, Dictionary<RoomOccupationRequest, List<RateDetails>> availableRateDetails)
        {
            var rateDetailsSet = ListHelper.GetCombinations(availableRateDetails.Select(rateDetails => rateDetails.Value).ToList())
                .Distinct()
                .ToList();
            
            FilterRoomDuplicatesByAllotment();
            OrderByMinTotalPrice();
            
            return rateDetailsSet;


            void FilterRoomDuplicatesByAllotment()
            {
                ListHelper.RemoveIfNot(rateDetailsSet, IsRateDetailsAvailableByAllotment);
            }
            
            
            bool IsRateDetailsAvailableByAllotment(List<RateDetails> rateDetails)
            {
                foreach (var roomGroup in rateDetails.GroupBy(rd => rd.Room))
                {
                    var roomsNumber = roomGroup.ToList().Count;
                    if (roomsNumber == 1)
                        continue;
                    if (!AvailabilityHelper.IsRoomAvailableByAllotment(availabilityRequest, roomGroup.Key, roomsNumber))
                        return false;
                }

                return true;
            }


            void OrderByMinTotalPrice()
            {
                rateDetailsSet = rateDetailsSet.OrderBy(rds => rds.Sum(rd => rd.PaymentDetails.TotalAmount.Amount)).ToList();
            }
        }
    }
}