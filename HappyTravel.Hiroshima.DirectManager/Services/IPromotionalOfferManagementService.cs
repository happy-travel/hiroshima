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
    }
}