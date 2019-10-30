using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Hiroshima.WebApi.Infrastructure
{
    public static class ProblemDetailsBuilder
    {
        public static Result<T, ProblemDetails> Fail<T>(string details,
                HttpStatusCode statusCode = HttpStatusCode.BadRequest)
                   => Result.Failure<T, ProblemDetails>(
                       new ProblemDetails
                       {
                           Detail = details,
                           Status = (int)statusCode
                       });
    }
}
