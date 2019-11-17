using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using NpgsqlTypes;

namespace Hiroshima.DirectContracts.Services
{
    public class DirectContractsRawDataFilter : IDirectContractsRawDataFilter
    {
        public List<RawAvailabilityData> FilterByRoomDetails(List<RawAvailabilityData> availabilityData, List<RoomDetails> roomDetails)
        {
            var availabilityResult = new List<RawAvailabilityData>();

            foreach (var details in roomDetails)
                for (var i = availabilityData.Count - 1; i >= 0; i--)
                {
                    var dataItem = availabilityData[i];

                    if (dataItem.RoomDetails.AdultsNumber == details.AdultsNumber &&
                        dataItem.RoomDetails.ChildrenNumber == details.ChildrenNumber)
                        if (dataItem.RoomDetails.ChildrenNumber == 0 ||
                            RangeContainsAny(dataItem.RoomDetails.ChildrenAges, details.ChildrenAges, true, false))
                        {
                            availabilityResult.Add(dataItem);
                            availabilityData.RemoveAt(i);
                        }
                }

            return availabilityResult;


            bool RangeContainsAny(NpgsqlRange<int> range, List<int> items, bool lowerBoundInclusive = true, bool upperBoundInclusive = true)
            {
                if (range.IsEmpty || !items.Any())
                    return false;

                foreach (var item in items)
                {
                    var inLowerBound = lowerBoundInclusive && range.LowerBound <= item ||
                        !lowerBoundInclusive && range.LowerBound < item;

                    var inUpperBound = upperBoundInclusive && item <= range.UpperBound ||
                        !upperBoundInclusive && item < range.UpperBound;

                    if (inLowerBound && inUpperBound)
                        return true;
                }

                return false;
            }
        }
    }
}