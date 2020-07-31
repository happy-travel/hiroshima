using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Infrastructure;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.Hiroshima.DirectManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.WebApi.Controllers.DirectManager
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management")]
    [Produces("application/json")]
    public class RatesManagementController : ControllerBase
    {
        public RatesManagementController(IRateManagementService rateManagementService)
        {
            _rateManagementService = rateManagementService;
        }


        /// <summary>
        /// Retrieves contract seasons 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="rates"></param>
        /// <returns></returns>
        [HttpPost("contracts/{contractId}/rates")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Rate>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddRates([FromRoute] int contractId, [FromBody] List<Rate> rates)
        { 
            var (_, isFailure, response, error) = await _rateManagementService.Add(contractId, rates);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        /// <summary>
        /// Retrieves contract rates
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="roomIds">List of room ids. E.g. roomIds=1,2,3 </param>
        /// <param name="seasonIds">List of season ids. E.g. seasonIds=1,2,3</param>
        /// <returns></returns>
        [HttpGet("contracts/{contractId}/rates")]
        [ProducesResponseType(typeof(List<Hiroshima.DirectManager.Models.Responses.Rate>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRates([FromRoute] int contractId, [FromQuery] string? roomIds = null, [FromQuery] string? seasonIds = null)
        {
            var (_, isRoomIdsFailure, roomIdsResult, checkRoomIdsError) = GetIDs(roomIds!);
            if (isRoomIdsFailure)
                return BadRequest(ProblemDetailsBuilder.Build(checkRoomIdsError));
            
            var (_, isSeasonIdsFailure, seasonIdsResult, checkSeasonIdsError) = GetIDs(seasonIds!);
            if (isSeasonIdsFailure)
                return BadRequest(ProblemDetailsBuilder.Build(checkSeasonIdsError));
            
            var (_, isFailure, response, error) = await _rateManagementService.Get(contractId, roomIdsResult, seasonIdsResult);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);


            Result<List<int>> GetIDs(string idsQuery)
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

        
        private readonly IRateManagementService _rateManagementService;
    }
}