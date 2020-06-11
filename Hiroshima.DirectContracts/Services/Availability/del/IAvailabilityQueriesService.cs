using System;
using System.Linq;
using Hiroshima.DirectContracts.Models.Internal;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services.Availability.del
{
    public interface IAvailabilityQueriesService
    {
        IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate, Point point,
            double distance);

        IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate,
            string accommodationName, Point point, double distance);

        IQueryable<RawAvailabilityData> GetAvailability(DateTime checkInDate, DateTime checkOutDate,
            string accommodationName, string localityName,
            string countyName);
    }
}