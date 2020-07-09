using System.Threading.Tasks;
using HappyTravel.Hiroshima.Data.Models.Accommodations;

namespace HappyTravel.Hiroshima.DirectContracts.Services.Management
{
    public interface IAccommodationManagementRepository
    {
        Task<Accommodation> GetAccommodation(int contractManagerId, int accommodationId);
        Task<Accommodation> AddAccommodation(Accommodation accommodation);
        Task<bool> DeleteAccommodation(int accommodationId);
        Task<bool> UpdateAccommodation(Accommodation accommodation);
    }
}