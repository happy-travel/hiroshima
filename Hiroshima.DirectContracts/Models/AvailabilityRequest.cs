using System;
using System.Collections.Generic;
using Hiroshima.Common.Models;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Models
{
    public class AvailabilityRequest
    {
        public Point Coordinates { get; set; }
        public double Radius { get; set; }
        public string LocationName { get; set; }
        public string CountryName { get; set; }
        public string LocalityName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public List<PermittedOccupancy> PermittedOccupancies{ get; set; }
        public string Nationality { get; set; }
        public string Residency { get; set; }
    }
}