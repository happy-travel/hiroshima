using System;
using System.Linq;
using System.Threading.Tasks;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Models;
using Hiroshima.DirectContracts.Models.Internal;
using Hiroshima.DirectContracts.Services.Availability.Helpers;
using Hiroshima.DirectContracts.Services.Availability.Predicates;
using Microsoft.EntityFrameworkCore;

namespace Hiroshima.DirectContracts.Services.Availability
{
    class DcAvailability : IDcAvailability
    {
        public DcAvailability(DirectContractsDbContext dbContext)
        {
            _requestCreator = new RequestCreator(dbContext);
            _responseCreator = new ResponseCreator();
        }
        

        public async Task<AvailabilityResponse> SearchAvailableAgreements(AvailabilityRequest availabilityRequest)
        {
            var dbAgreements = EmptyAgreement;
            
            if (!availabilityRequest.Coordinates.IsEmpty)
            {
                if (string.IsNullOrWhiteSpace(availabilityRequest.LocationName))
                {
                    dbAgreements = _requestCreator.SearchAvailability(
                            availabilityRequest.CheckInDate,
                            availabilityRequest.CheckOutDate)
                        .Where(InDbExecutionPredicates.FilterByCoordinatesAndDistance(
                            availabilityRequest.Coordinates,
                            availabilityRequest.Radius))
                        .Where(InDbExecutionPredicates.FilterByPermittedOccupancies(
                            availabilityRequest.PermittedOccupancies));
                }
                else
                {
                    throw new NotImplementedException("Search by the accommodation name in the area");
                }
            }
            else if (!string.IsNullOrWhiteSpace(availabilityRequest.LocationName) ||
                !string.IsNullOrWhiteSpace(availabilityRequest.LocalityName) ||
                !string.IsNullOrWhiteSpace(availabilityRequest.CountryName))
            {
                throw new NotImplementedException("Search by the accommodation name, locality name");
            }
            
            return _responseCreator.CreateAvailabilityResponse(await dbAgreements.ToListAsync(), availabilityRequest.CheckInDate, availabilityRequest.CheckOutDate);
        }


        private readonly ResponseCreator _responseCreator;
        private readonly RequestCreator _requestCreator;
        private static readonly IQueryable<RawAgreementData> EmptyAgreement = Enumerable.Empty<RawAgreementData>().AsQueryable();
    }
}
