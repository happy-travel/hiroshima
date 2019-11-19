using System;
using System.Linq;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services.Availability
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