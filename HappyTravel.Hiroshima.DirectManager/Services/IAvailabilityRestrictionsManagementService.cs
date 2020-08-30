using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAvailabilityRestrictionsManagementService
    {
        public Task<Result<List<Models.Responses.AvailabilityRestriction>>> Set(int contractId, List<Models.Requests.AvailabilityRestriction> availabilityRestrictions);
        
        public Task<Result<Models.Responses.AvailabilityRestriction>> Get(int contractId, List<int> roomIds, DateTime? fromDate, DateTime? toDate, AvailabilityRestrictions? restrictions);
    }
}