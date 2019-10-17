using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Hiroshima.Common.Models;
using Hiroshima.DbData;
using Hiroshima.DirectContracts.Models.Internal;
using NetTopologySuite.Geometries;

namespace Hiroshima.DirectContracts.Services.Availability.Predicates
{
    static class InDbExecutionPredicates
    {
        public static Expression<Func<RawAgreementData, bool>> FilterByCoordinatesAndDistance(Point coordinates,
            double radius)
        {
            return row => DirectContractsDbContext.StDistanceSphere(coordinates, row.Coordinates) <= radius;
        }
        
        public static Expression<Func<RawAgreementData, bool>> FilterByPermittedOccupancies(
            List<PermittedOccupancy> permittedOccupancies)
        {
            return row => permittedOccupancies
                              .Select(permittedOccupancy => permittedOccupancy.AdultsNumber)
                              .Any(iCount => iCount == row.PermittedOccupancy.AdultsNumber) &&
                          permittedOccupancies
                              .Select(permittedOccupancy => permittedOccupancy.ChildrenNumber)
                              .Any(iCount => iCount == row.PermittedOccupancy.ChildrenNumber);
        }
    }
}