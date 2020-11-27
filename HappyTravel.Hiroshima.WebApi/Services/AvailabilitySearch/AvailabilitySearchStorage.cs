using System;
using System.Threading.Tasks;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AvailabilitySearchStorage : IAvailabilitySearchStorage
    {
        public AvailabilitySearchStorage(IDistributedFlow distributedFlow)
        {
            _distributedFlow = distributedFlow;
        }


        public Task AddAvailabilityRequest(string availabilityId, in AvailabilityRequest availabilityRequest) => _distributedFlow.SetAsync(BuildAvailabilityRequestKey(availabilityId), availabilityRequest, CacheExpirationTime);


        public Task<AvailabilityRequest> GetAvailabilityRequest(string availabilityId) => _distributedFlow.GetAsync<AvailabilityRequest>(BuildAvailabilityRequestKey(availabilityId));


        public Task RemoveAvailabilityRequest(string availabilityId) => _distributedFlow.RemoveAsync(BuildAvailabilityRequestKey(availabilityId));


        public Task AddAccommodationAvailability(in AccommodationAvailability accommodationAvailability) => _distributedFlow.SetAsync(BuildAccommodationAvailabilityKey(accommodationAvailability.AvailabilityId), accommodationAvailability, CacheExpirationTime);


        public Task<AccommodationAvailability> GetAccommodationAvailability(string availabilityId) => _distributedFlow.GetAsync<AccommodationAvailability>(BuildAccommodationAvailabilityKey(availabilityId));
        
        
        public Task RemoveAvailability(string availabilityId) => _distributedFlow.RemoveAsync(BuildAccommodationAvailabilityKey(availabilityId));

        
        private string BuildAvailabilityRequestKey(string availabilityId) => _distributedFlow.BuildKey(nameof(AvailabilitySearchStorage), $"{nameof(BuildAvailabilityRequestKey)}", availabilityId);
        
        
        private string BuildAccommodationAvailabilityKey(string availabilityId) => _distributedFlow.BuildKey(nameof(AvailabilitySearchStorage), $"{nameof(BuildAccommodationAvailabilityKey)}", availabilityId);
        
        
        private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(15);
        private readonly IDistributedFlow _distributedFlow;
    }
}