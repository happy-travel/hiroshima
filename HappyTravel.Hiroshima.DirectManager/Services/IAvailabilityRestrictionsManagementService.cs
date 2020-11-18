using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IAvailabilityRestrictionsManagementService
    {
        Task<Result<List<Models.Responses.AvailabilityRestriction>>> Set(int contractId, List<Models.Requests.AvailabilityRestriction> availabilityRestrictions);
        
        Task<Result<List<Models.Responses.AvailabilityRestriction>>> Get(int contractId, int skip, int top, List<int> roomIds, DateTime? fromDate, DateTime? toDate, AvailabilityRestrictions? restriction);

        Task<Result> Remove(int contractId, List<int> availabilityRestrictionIds);
    }
}