using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IPromotionalOfferManagementService
    {
        Task<Result<List<Models.Responses.PromotionalOffer>>> Get(int contractId, int skip, int top, List<int> roomIds, DateTime? validFrom = null, DateTime? validTo = null);

        Task<Result<List<Models.Responses.PromotionalOffer>>> Add(int contractId, List<Models.Requests.PromotionalOffer> promotionalOffers);

        Task<Result> Remove(int contractId, List<int> promotionalOfferIds);

        Task<Result<List<Models.Responses.PromotionalOfferStopSalePeriod>>> AddStopSalePeriods(int contractId, List<Models.Requests.PromotionalOfferStopSale> stopSalePeriods);

        Task<Result<List<Models.Responses.PromotionalOfferStopSalePeriod>>> GetStopSalePeriods(int contractId, int skip, int top, List<int> roomIds, DateTime? fromDate, DateTime? toDate);
        
        Task<Result> RemoveStopSalePeriods(int contractId, List<int> stopSalePeriodIds);
    }
}