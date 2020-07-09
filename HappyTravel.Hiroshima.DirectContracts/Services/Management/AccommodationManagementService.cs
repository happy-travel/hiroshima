using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public class AccommodationManagementService: IAccommodationManagementService
    {
        public AccommodationManagementService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public async Task<Accommodation> GetAccommodation(int userId, int accommodationId) 
            => await _dbContext.Accommodations.SingleOrDefaultAsync(a => a.Id == accommodationId && a.UserId == userId);

        
        public async Task<Accommodation> AddAccommodation(Accommodation accommodation)
        { 
            var entry = _dbContext.Accommodations.Add(accommodation);
            await _dbContext.SaveChangesAsync();
            entry.State = EntityState.Detached;
            return entry.Entity;
        }

        
        public Task<bool> DeleteAccommodation(int accommodationId)
        {
            throw new System.NotImplementedException();
        }

        
        public Task<bool> UpdateAccommodation(Accommodation accommodation)
        {
            throw new System.NotImplementedException();
        }

        
        private readonly DirectContractsDbContext _dbContext;
    }
}