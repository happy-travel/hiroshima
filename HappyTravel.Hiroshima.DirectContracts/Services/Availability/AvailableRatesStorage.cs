using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.Hiroshima.DirectContracts.Models;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Availability
{
    public class AvailableRatesStorage : IAvailableRatesStorage
    {
        public AvailableRatesStorage(IDoubleFlow doubleFlow)
        {
            _doubleFlow = doubleFlow;
        }
        
        
        public Task Add(List<AvailableRates> rates) => Task.WhenAll(rates.Select(ar => _doubleFlow.SetAsync(BuildKey(ar.Id), ar, CacheExpirationTime)));
        

        public ValueTask<AvailableRates> Get(Guid id) => _doubleFlow.GetAsync<AvailableRates>(BuildKey(id), CacheExpirationTime);
        

        private string BuildKey(Guid id) => _doubleFlow.BuildKey(nameof(AvailableRatesStorage), id.ToString());
        private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(15);
        
        private readonly IDoubleFlow _doubleFlow;
    }
}