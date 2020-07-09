using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentValidation;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ContractManagementService: IContractManagementService
    {
        public ContractManagementService(IUserContextService userContextService, DirectContracts.Services.Management.IContractManagementService contractManagement, DirectContracts.Services.Management.IAccommodationManagementService accommodationManagement)
        {
            _userContext = userContextService;
            _contractManagement = contractManagement;
            _accommodationManagement = accommodationManagement;
        }
        
        
        public async Task<Result<Models.Responses.Contract>> GetContract(int contractId)
        {
            return await _userContext.GetUser()
                .Bind(async user =>
                {
                    var contract = await _contractManagement.GetContract(user.Id, contractId);
                    
                    if (contract is null) 
                        return Result.Failure<Models.Responses.Contract>($"Failed to get the contract with {nameof(contractId)} '{contractId}'");

                    var relatedAccommodationId =
                        (await _contractManagement.GetRelatedAccommodations(user.Id, contractId)).Single().Id;

                    return Result.Ok(new Models.Responses.Contract
                    {
                        Id = contract.Id,
                        Name = contract.Name,
                        Description = contract.Description,
                        ValidFrom = contract.ValidFrom,
                        ValidTo = contract.ValidTo,
                        AccommodationId = relatedAccommodationId
                    });
                });
        }

        
        public async Task<Result<List<Models.Responses.Contract>>> GetContracts()
        {
            return await _userContext.GetUser()
                .Bind(async user =>
                {
                    var contracts = (await _contractManagement.GetContracts(user.Id)).ToList();
                    if (!contracts.Any())
                        return Result.Ok(new List<Models.Responses.Contract>());

                    var contractIds = contracts.Select(c => c.Id).ToList();
                    var contractsAccommodationRelations = (await _contractManagement.GetContractRelations(user.Id, contractIds))
                        .ToDictionary(k=>k.ContractId);

                    var response = contracts.Select(c =>
                        CreateContractResponse(c, contractsAccommodationRelations[c.Id].AccommodationId)).ToList();
                    return Result.Ok(response);
                });
        }

        
        public async Task<Result<Models.Responses.Contract>> AddContract(Models.Requests.Contract contract)
        {
            return await _userContext.GetUser()
                .Tap(user => ValidateContract(contract))
                .Ensure(user => IsUserAccommodation(user.Id, contract.AccommodationId),
                    $"Accommodation with {nameof(contract.AccommodationId)} '{contract.AccommodationId}' does not belong to the user")
                .Bind(async user =>
                {
                    var newContract = await _contractManagement.AddContract(
                        new Contract
                        {
                            Name = contract.Name,
                            Description = contract.Description,
                            ValidFrom = contract.ValidFrom,
                            ValidTo = contract.ValidTo,
                            UserId = user.Id
                        }, contract.AccommodationId);
                    return !newContract.Id.Equals(default)
                        ? Result.Ok(CreateContractResponse(newContract, contract.AccommodationId))
                        : Result.Failure<Models.Responses.Contract>("Failed to add contract");
                });
        }

        
        public async Task<Result> UpdateContract(int contractId, Models.Requests.Contract contract)
        {
            return await _userContext.GetUser()
                .Tap(user => ValidateContract(contract))
                .Ensure(user => IsUserContract(user.Id, contractId),
                    $"Contract with {nameof(contractId)} '{contractId}' does not belong to the user"
                ).Bind(async user =>
                {
                    await _contractManagement.UpdateContract(new Contract
                    {
                        Id = contractId,
                        Name = contract.Name,
                        Description = contract.Description,
                        UserId = user.Id,
                        ValidFrom = contract.ValidFrom,
                        ValidTo = contract.ValidTo
                    });
                    return Result.Ok();
                });
        }

        
        public async Task<Result> DeleteContract(int contractId)
        {
            return await _userContext.GetUser()
                .Tap(user => _contractManagement.DeleteContract(user.Id, contractId));
        }


        private Result ValidateContract(Models.Requests.Contract contract)
        {
            var result = GenericValidator<Models.Requests.Contract>.Validate(v =>
            {
                v.RuleFor(c => c.Name).NotEmpty();
                v.RuleFor(c => c.Description).NotEmpty();
                v.RuleFor(c => c.AccommodationId).NotEmpty();
                v.RuleFor(c => c.ValidFrom).LessThan(c=> c.ValidTo);
            }, contract);
            return result;
        }

        
        private async Task<bool> IsUserAccommodation(int userId, int accommodationId)
        {
            var accommodation = await _accommodationManagement.GetAccommodation(userId, accommodationId);
            return !(accommodation is null);
        }
        
        
        private async Task<bool> IsUserContract(int userId, int contractId)
        {
            var contract = await _contractManagement.GetContract(userId, contractId);
            return !(contract is null);
        }


        private Models.Responses.Contract CreateContractResponse(Contract contract, int accommodationId)
            => new Models.Responses.Contract
            {
                Id = contract.Id,
                AccommodationId = accommodationId,
                Name = contract.Name,
                Description = contract.Description,
                ValidFrom = contract.ValidFrom,
                ValidTo = contract.ValidTo
            };
        
        
        private readonly IUserContextService _userContext;
        private readonly DirectContracts.Services.Management.IContractManagementService _contractManagement;
        private readonly DirectContracts.Services.Management.IAccommodationManagementService _accommodationManagement;
    }
}