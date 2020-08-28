using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Extensions;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public class AccommodationManagementRepository: IAccommodationManagementRepository
    {
        public AccommodationManagementRepository(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public async Task<Accommodation> GetAccommodation(int contractManagerId, int accommodationId) 
            => await _dbContext.Accommodations.SingleOrDefaultAsync(a => a.Id == accommodationId && a.ContractManagerId == contractManagerId);

        
        public async Task<List<Room>> GetRooms(int accommodationId) 
            => await _dbContext.Rooms.Where(r => r.AccommodationId == accommodationId).ToListAsync();

        
        private readonly DirectContractsDbContext _dbContext;
    }
}