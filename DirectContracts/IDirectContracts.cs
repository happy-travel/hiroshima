using Hiroshima.DirectContracts.Services;

namespace Hiroshima.DirectContracts
{
    public interface IDirectContracts
    {
       IAvailabilitySearch AvailabilitySearch { get; }
       ILocations Locations { get; }
    }
}
