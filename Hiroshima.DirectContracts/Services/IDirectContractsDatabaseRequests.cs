using System;
using System.Linq;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services
{
    public interface IDirectContractsDatabaseRequests
    {
        IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate, Point point, double distance);

        IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate, string accommodationName, Point point, double distance);


        IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate, string accommodationName, string localityName,
            string countyName);
    }
}