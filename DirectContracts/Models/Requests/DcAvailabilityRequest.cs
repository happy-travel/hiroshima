using System;
using System.Collections.Generic;
using Hiroshima.DirectContracts.Models.Common;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Models.Requests
{
    public class DcAvailabilityRequest
    {
        public Point Coordinates { get; set; }
        public double SearchRadiusInMeters { get; set; }
        public string LocationName { get; set; }
        public string CountryName { get; set; }
        public string LocalityName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public List<DcPermittedOccupancy> PermittedOccupancies{ get; set; }
        public string Nationality { get; set; }
        public string Residency { get; set; }
    }
}