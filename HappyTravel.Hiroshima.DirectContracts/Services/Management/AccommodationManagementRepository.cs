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

        
        public async Task<List<Room>> GetRooms(int contractManagerId, int accommodationId) =>
            await _dbContext.Rooms
                .Join(_dbContext.Accommodations, room => room.AccommodationId, accommodation => accommodation.Id,
                    (room, accommodation) => new {room, accommodation})
                .Where(roomAndAccommodations =>
                    roomAndAccommodations.accommodation.ContractManagerId == contractManagerId &&
                    roomAndAccommodations.accommodation.Id == accommodationId)
                .Select(roomAndAccommodation => roomAndAccommodation.room)
                .ToListAsync();
        
        
        
        public async Task UpdateRooms(List<Room> rooms)
        { 
            _dbContext.Rooms.UpdateRange(rooms);
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachEntries(rooms);
        }

        
        public async Task DeleteRooms(List<int> roomIds)
        {
            var roomsToDelete = roomIds.Select(id => new Room {Id = id});
            _dbContext.Rooms.RemoveRange(roomsToDelete);
            await _dbContext.SaveChangesAsync();
        }
        
        
        private readonly DirectContractsDbContext _dbContext;
    }
}