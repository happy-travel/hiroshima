using System;
using System.Threading.Tasks;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;

namespace HappyTravel.Hiroshima.WebApi.Services.AvailabilitySearch
{
    public class AvailabilityIdMatchingService : IAvailabilityIdMatchingService
    {
        public AvailabilityIdMatchingService(IDistributedFlow distributedFlow)
        {
            _distributedFlow = distributedFlow;
        }
        
        
        public Task SetAccommodationAvailabilityId(string wideAvailabilityId, string accommodationAvailabilityId)
            => _distributedFlow.SetAsync(BuildKey(wideAvailabilityId), accommodationAvailabilityId, CacheExpirationTime);


        public Task<string> GetAccommodationAvailabilityId(string wideAvailability) 
            => _distributedFlow.GetAsync<string>(BuildKey(wideAvailability));
        
        
        private string BuildKey(string availabilityId) => _distributedFlow.BuildKey(nameof(AvailabilityIdMatchingService), availabilityId);

        
        private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(15);
        private readonly IDistributedFlow _distributedFlow;
    }
}