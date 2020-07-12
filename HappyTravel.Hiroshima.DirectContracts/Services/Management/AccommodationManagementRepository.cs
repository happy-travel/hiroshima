using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data;
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

        
        public async Task<Accommodation> AddAccommodation(Accommodation accommodation)
        { 
            var entry = _dbContext.Accommodations.Add(accommodation);
            await _dbContext.SaveChangesAsync();
            entry.State = EntityState.Detached;
            return entry.Entity;
        }

        
        public async Task DeleteAccommodationAndRooms(int contractManagerId, int accommodationId)
        {
            var accommodation = await GetAccommodation(contractManagerId, accommodationId);
            if (accommodation is null)
                return;
            
            var rooms = await GetRooms(accommodation.Id);
            if (rooms.Any())
                await DeleteRooms(rooms.Select(r => r.Id).ToList());

            await DeleteAccommodation(accommodationId);
        }

        
        public async Task UpdateAccommodation(Accommodation accommodation)
        {
            var entry = _dbContext.Accommodations.Update(accommodation);
            await _dbContext.SaveChangesAsync();
            entry.State = EntityState.Detached;
        }
        

        public async Task<List<Room>> GetRooms(int accommodationId) 
            => await _dbContext.Rooms.Where(r => r.AccommodationId == accommodationId).ToListAsync();


        public async Task<List<Room>> AddRooms(List<Room> rooms)
        {
            _dbContext.Rooms.AddRange(rooms);
            await _dbContext.SaveChangesAsync();
            DefineIdAndDetach(rooms);
            return rooms;
        }

        
        public async Task UpdateRooms(List<Room> rooms)
        { 
            _dbContext.Rooms.UpdateRange(rooms);
            await _dbContext.SaveChangesAsync();
            DefineIdAndDetach(rooms);
        }

        
        public async Task DeleteRooms(List<int> roomIds)
        {
            var roomsToDelete = roomIds.Select(id => new Room {Id = id});
            _dbContext.Rooms.RemoveRange(roomsToDelete);
            await _dbContext.SaveChangesAsync();
        }
        
        private async Task DeleteAccommodation(int accommodationId)
        {
            var accommodationToDelete = new Accommodation {Id = accommodationId};
            _dbContext.Accommodations.RemoveRange(accommodationToDelete);
            await _dbContext.SaveChangesAsync();
        }
        
        
        private void DefineIdAndDetach(List<Room> rooms)
        {
            foreach (var room in rooms)
            {
                var entry = _dbContext.Entry(room);
                entry.State = EntityState.Detached;
                room.Id = entry.Entity.Id;
            }
        }
        
        
        private readonly DirectContractsDbContext _dbContext;
    }
}