using System;
using System.Threading.Tasks;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AvailabilitySearchStore : IAvailabilitySearchStore
    {
        public AvailabilitySearchStore(IDistributedFlow distributedFlow)
        {
            _distributedFlow = distributedFlow;
        }


        public Task AddAvailabilityRequest(string availabilityId, in AvailabilityRequest availabilityRequest) => _distributedFlow.SetAsync(BuildKey(availabilityId), availabilityRequest, CacheExpirationTime);


        public Task<AvailabilityRequest> GetAvailabilityRequest(string availabilityId) => _distributedFlow.GetAsync<AvailabilityRequest>(BuildKey(availabilityId));


        public Task AddAccommodationAvailability(AccommodationAvailability accommodationAvailability) => _distributedFlow.SetAsync(BuildKey(accommodationAvailability.AvailabilityId), accommodationAvailability, CacheExpirationTime);


        public Task<AccommodationAvailability> GetAccommodationAvailability(string availabilityId) => _distributedFlow.GetAsync<AccommodationAvailability>(BuildKey(availabilityId));
        
        
        public Task RemoveAvailability(string availabilityId) => _distributedFlow.RemoveAsync(BuildKey(availabilityId));

        
        private string BuildKey(string availabilityId) => _distributedFlow.BuildKey(nameof(AvailabilitySearchStore), availabilityId);
        
        
        private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(15);
        private readonly IDistributedFlow _distributedFlow;
    }
}