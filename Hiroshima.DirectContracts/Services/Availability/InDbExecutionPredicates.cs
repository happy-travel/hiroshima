using System;
using System.Linq.Expressions;
using HappyTravel.EdoContracts.Accommodations.Internals;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Models.RawAvailiability;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services.Availability
{
    static class InDbExecutionPredicates
    {
        public static Expression<Func<RawAvailability, bool>> FilterByCoordinatesAndDistance(Point coordinates,
            double radius)
        {
            return item => DirectContractsDbContext.StDistanceSphere(coordinates, item.Location.Coordinates) <= radius;
        }


        public static Expression<Func<RawAvailability, bool>> FilterByRoomDetails(
            RoomDetails roomDetails)
        {
            return item => item.PermittedOccupancy.AdultsNumber >= roomDetails.AdultsNumber &&
                           item.PermittedOccupancy.ChildrenNumber >= roomDetails.ChildrenNumber;
        }


        public static Expression<Func<RawAvailability, bool>> SearchByAccommodationName(string name)
        {
            //Todo find a way to set json as a document.
            //Todo combine predicates
            return item =>
                EF.Functions.ToTsVector("simple", item.Accommodation.Name.Ar).Matches(name) ||
                EF.Functions.ToTsVector("english", item.Accommodation.Name.En).Matches(name) ||
                EF.Functions.ToTsVector("simple", item.Accommodation.Name.Es).Matches(name) ||
                EF.Functions.ToTsVector("simple", item.Accommodation.Name.Ru).Matches(name);
        }


        public static Expression<Func<RawAvailability, bool>> SearchByAccommodationLocality(string name)
        {
            return item => EF.Functions.ILike(item.Accommodation.Location.Locality.Name.Ar, $"%{name}%") ||
                           EF.Functions.ILike(item.Accommodation.Location.Locality.Name.En, $"%{name}%") ||
                           EF.Functions.ILike(item.Accommodation.Location.Locality.Name.Es, $"%{name}%") ||
                           EF.Functions.ILike(item.Accommodation.Location.Locality.Name.Ru, $"%{name}%");
        }


        public static Expression<Func<RawAvailability, bool>> SearchByAccommodationCountry(string name)
        {
            return item => EF.Functions.ILike(item.Accommodation.Location.Locality.Country.Name.Ar, $"%{name}%") ||
                           EF.Functions.ILike(item.Accommodation.Location.Locality.Country.Name.En, $"%{name}%") ||
                           EF.Functions.ILike(item.Accommodation.Location.Locality.Country.Name.Es, $"%{name}%") ||
                           EF.Functions.ILike(item.Accommodation.Location.Locality.Country.Name.Ru, $"%{name}%");
        }
    }
}