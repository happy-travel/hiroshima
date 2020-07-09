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
        public ContractManagementService(IContractManagerContextService contractManagerContextService, DirectContracts.Services.Management.IContractManagementRepository contractManagementRepository, DirectContracts.Services.Management.IAccommodationManagementRepository accommodationManagementRepository)
        {
            _contractManagerContext = contractManagerContextService;
            _contractManagementRepository = contractManagementRepository;
            _accommodationManagementRepository = accommodationManagementRepository;
        }
        
        
        public async Task<Result<Models.Responses.Contract>> Get(int contractId)
        {
            return await _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var contract = await _contractManagementRepository.GetContract(contractManager.Id, contractId);
                    
                    if (contract is null) 
                        return Result.Failure<Models.Responses.Contract>($"Failed to get the contract with {nameof(contractId)} '{contractId}'");

                    var relatedAccommodationId =
                        (await _contractManagementRepository.GetRelatedAccommodations(contractManager.Id, contractId)).Single().Id;

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

        
        public async Task<Result<List<Models.Responses.Contract>>> Get()
        {
            return await _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var contracts = (await _contractManagementRepository.GetContracts(contractManager.Id)).ToList();
                    if (!contracts.Any())
                        return Result.Ok(new List<Models.Responses.Contract>());

                    var contractIds = contracts.Select(c => c.Id).ToList();
                    var contractsAccommodationRelations = (await _contractManagementRepository.GetContractRelations(contractManager.Id, contractIds))
                        .ToDictionary(k=>k.ContractId);

                    var response = contracts.Select(c =>
                        CreateResponse(c, contractsAccommodationRelations[c.Id].AccommodationId)).ToList();
                    return Result.Ok(response);
                });
        }

        
        public async Task<Result<Models.Responses.Contract>> Add(Models.Requests.Contract contract)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(user => Validate(contract))
                .Ensure(user => DoesAccommodationBelongToContractManager(user.Id, contract.AccommodationId),
                    $"Accommodation with {nameof(contract.AccommodationId)} '{contract.AccommodationId}' does not belong to the contract manager")
                .Bind(async contractManager =>
                {
                    var newContract = await _contractManagementRepository.AddContract(
                        new Contract
                        {
                            Name = contract.Name,
                            Description = contract.Description,
                            ValidFrom = contract.ValidFrom,
                            ValidTo = contract.ValidTo,
                            ContractManagerId = contractManager.Id
                        }, contract.AccommodationId);
                    return !newContract.Id.Equals(default)
                        ? Result.Ok(CreateResponse(newContract, contract.AccommodationId))
                        : Result.Failure<Models.Responses.Contract>("Failed to add contract");
                });
        }

        
        public async Task<Result> Update(int contractId, Models.Requests.Contract contract)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(user => Validate(contract))
                .Ensure(user => DoesContractBelongToContractManager(user.Id, contractId),
                    $"Contract with {nameof(contractId)} '{contractId}' does not belong to the contract manager"
                ).Bind(async contractManager =>
                {
                    await _contractManagementRepository.UpdateContract(new Contract
                    {
                        Id = contractId,
                        Name = contract.Name,
                        Description = contract.Description,
                        ContractManagerId = contractManager.Id,
                        ValidFrom = contract.ValidFrom,
                        ValidTo = contract.ValidTo
                    });
                    return Result.Ok();
                });
        }

        
        public async Task<Result> Remove(int contractId)
        {
            return await _contractManagerContext.GetContractManager()
                .Tap(user => _contractManagementRepository.DeleteContract(user.Id, contractId));
        }


        private Result Validate(Models.Requests.Contract contract)
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

        
        private async Task<bool> DoesAccommodationBelongToContractManager(int contractManagerId, int accommodationId)
            => !(await _accommodationManagementRepository.GetAccommodation(contractManagerId, accommodationId) is null);
        
        
        
        private async Task<bool> DoesContractBelongToContractManager(int contractManagerId, int contractId)
            => !(await _contractManagementRepository.GetContract(contractManagerId, contractId) is null);
        


        private Models.Responses.Contract CreateResponse(Contract contract, int accommodationId)
            => new Models.Responses.Contract
            {
                Id = contract.Id,
                AccommodationId = accommodationId,
                Name = contract.Name,
                Description = contract.Description,
                ValidFrom = contract.ValidFrom,
                ValidTo = contract.ValidTo
            };
        
        
        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContracts.Services.Management.IContractManagementRepository _contractManagementRepository;
        private readonly DirectContracts.Services.Management.IAccommodationManagementRepository _accommodationManagementRepository;
    }
}