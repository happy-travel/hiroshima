using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.Common.Infrastructure.Utilities
{
    public static class QueryStringUtilities
    {
        public static Result<List<int>> GetIDs(string idsQuery)
        {
            if (string.IsNullOrWhiteSpace(idsQuery))
                return Result.Success(new List<int>());

            var idLiterals = idsQuery.Split(',')
                .Select(id => id.Trim());

            var ids = new List<int>(idLiterals.Count());
            foreach (var idLiteral in idLiterals)
            {
                if (!int.TryParse(idLiteral, out var id))
                    return Result.Failure<List<int>>($"Invalid ids '{idsQuery}'");

                ids.Add(id);
            }

            return Result.Success(ids);
        }
    }
}