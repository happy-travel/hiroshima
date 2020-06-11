using System;
using System.Linq.Expressions;
using Hiroshima.DirectContracts.Models.Internal;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services.Availability.del
{
    internal static class AvailabilitySearchExpressions
    {
        public static Expression<Func<RawAvailabilityData, bool>> FilterByCoordinatesAndDistance(Point coordinates,
            double radius)
        {
           // return item => DirectContractsDbContext.GetDistance(coordinates, item.Location.Coordinates) <= radius;
           throw new NotImplementedException();
        }


        public static Expression<Func<RawAvailabilityData, bool>> FilterByAccommodationName(string name)
        {
            //Todo find a way to set json as a document.
            //Todo combine predicates
          /*  return item =>
                EF.Functions.ToTsVector("simple", item.Accommodation.Name.Ar).Matches(name) ||
                EF.Functions.ToTsVector("english", item.Accommodation.Name.En).Matches(name) ||
                EF.Functions.ToTsVector("simple", item.Accommodation.Name.Es).Matches(name) ||
                EF.Functions.ToTsVector("simple", item.Accommodation.Name.Ru).Matches(name);*/
          throw new NotImplementedException();
        }


        public static Expression<Func<RawAvailabilityData, bool>> FilterByAccommodationLocality(string name)
        {
           /* return item => EF.Functions.ILike(item.Accommodation.Location.Locality.Name.Ar, $"%{name}%") ||
                EF.Functions.ILike(item.Accommodation.Location.Locality.Name.En, $"%{name}%") ||
                EF.Functions.ILike(item.Accommodation.Location.Locality.Name.Es, $"%{name}%") ||
                EF.Functions.ILike(item.Accommodation.Location.Locality.Name.Ru, $"%{name}%");*/
           throw new NotImplementedException();
        }


        public static Expression<Func<RawAvailabilityData, bool>> FilterByAccommodationCountry(string name)
        {
           /* return item => EF.Functions.ILike(item.Accommodation.Location.Locality.Country.Name.Ar, $"%{name}%") ||
                EF.Functions.ILike(item.Accommodation.Location.Locality.Country.Name.En, $"%{name}%") ||
                EF.Functions.ILike(item.Accommodation.Location.Locality.Country.Name.Es, $"%{name}%") ||
                EF.Functions.ILike(item.Accommodation.Location.Locality.Country.Name.Ru, $"%{name}%");*/
           throw new NotImplementedException();
        }
    }
}