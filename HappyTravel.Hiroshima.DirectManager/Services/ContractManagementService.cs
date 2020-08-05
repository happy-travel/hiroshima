using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentValidation;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.DirectManager.Infrastructure;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ContractManagementService : IContractManagementService
    {
        public ContractManagementService(IContractManagerContextService contractManagerContextService,
            DirectContracts.Services.Management.IContractManagementRepository contractManagementRepository,
            DirectContracts.Services.Management.IAccommodationManagementRepository accommodationManagementRepository)
        {
            _contractManagerContext = contractManagerContextService;
            _contractManagementRepository = contractManagementRepository;
            _accommodationManagementRepository = accommodationManagementRepository;
        }


        public Task<Result<Models.Responses.ContractResponse>> Get(int contractId)
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var contract = await _contractManagementRepository.GetContract(contractId, contractManager.Id);

                    if (contract is null)
                        return Result.Failure<Models.Responses.ContractResponse>($"Failed to get the contract with {nameof(contractId)} '{contractId}'");

                    var relatedAccommodationId = (await _contractManagementRepository.GetRelatedAccommodations(contractId, contractManager.Id)).Single().Id;

                    return Result.Success(new Models.Responses.ContractResponse(id: contract.Id, name: contract.Name, description: contract.Description,
                        validFrom: contract.ValidFrom, validTo: contract.ValidTo, accommodationId: relatedAccommodationId));
                });
        }


        public Task<Result<List<Models.Responses.ContractResponse>>> Get()
        {
            return _contractManagerContext.GetContractManager()
                .Bind(async contractManager =>
                {
                    var contracts = (await _contractManagementRepository.GetContracts(contractManager.Id)).ToList();
                    if (!contracts.Any())
                        return Result.Success(new List<Models.Responses.ContractResponse>());

                    var contractIds = contracts.Select(c => c.Id).ToList();
                    var contractsAccommodationRelations =
                        (await _contractManagementRepository.GetContractRelations(contractManager.Id, contractIds)).ToDictionary(k => k.ContractId);

                    var response = contracts.Select(c => CreateResponse(c, contractsAccommodationRelations[c.Id].AccommodationId)).ToList();
                    return Result.Success(response);
                });
        }


        public Task<Result<Models.Responses.ContractResponse>> Add(Models.Requests.ContractRequest contractRequest)
        {
            return _contractManagerContext.GetContractManager()
                .Tap(contractManager => Validate(contractRequest))
                .Ensure(contractManager => DoesAccommodationBelongToContractManager(contractManager.Id, contractRequest.AccommodationId),
                    $"Accommodation with {nameof(contractRequest.AccommodationId)} '{contractRequest.AccommodationId}' does not belong to the contract manager")
                .Bind(async contractManager =>
                {
                    var newContract = await _contractManagementRepository.AddContract(new Contract
                    {
                        Name = contractRequest.Name,
                        Description = contractRequest.Description,
                        ValidFrom = contractRequest.ValidFrom,
                        ValidTo = contractRequest.ValidTo,
                        ContractManagerId = contractManager.Id
                    }, contractRequest.AccommodationId);
                    return !newContract.Id.Equals(default)
                        ? Result.Success(CreateResponse(newContract, contractRequest.AccommodationId))
                        : Result.Failure<Models.Responses.ContractResponse>("Failed to add the contract");
                });
        }


        public Task<Result> Update(int contractId, Models.Requests.ContractRequest contractRequest)
        {
            return _contractManagerContext.GetContractManager()
                .Tap(contractManager => Validate(contractRequest))
                .Ensure(contractManager => DoesContractBelongToContractManager(contractManager.Id, contractId),
                    $"Contract with {nameof(contractId)} '{contractId}' does not belong to the contract manager")
                .Bind(async contractManager =>
                {
                    await _contractManagementRepository.UpdateContract(new Contract
                    {
                        Id = contractId,
                        Name = contractRequest.Name,
                        Description = contractRequest.Description,
                        ContractManagerId = contractManager.Id,
                        ValidFrom = contractRequest.ValidFrom,
                        ValidTo = contractRequest.ValidTo
                    });
                    return Result.Success();
                });
        }


        public async Task<Result> Remove(int contractId)
        {
            return await _contractManagerContext.GetContractManager().Tap(contractManager => _contractManagementRepository.DeleteContract(contractId, contractManager.Id));
        }


        private Result Validate(Models.Requests.ContractRequest contractRequest)
        {
            var result = GenericValidator<Models.Requests.ContractRequest>.Validate(v =>
            {
                v.RuleFor(c => c.Name).NotEmpty();
                v.RuleFor(c => c.Description).NotEmpty();
                v.RuleFor(c => c.AccommodationId).NotEmpty();
                v.RuleFor(c => c.ValidFrom).LessThan(c => c.ValidTo);
            }, contractRequest);
            return result;
        }


        private async Task<bool> DoesAccommodationBelongToContractManager(int contractManagerId, int accommodationId)
            => await _accommodationManagementRepository.GetAccommodation(contractManagerId, accommodationId) != null;


        private async Task<bool> DoesContractBelongToContractManager(int contractManagerId, int contractId)
            => await _contractManagementRepository.GetContract(contractId, contractManagerId) != null;


        private Models.Responses.ContractResponse CreateResponse(Contract contract, int accommodationId)
            => new Models.Responses.ContractResponse(id: contract.Id, accommodationId: accommodationId, name: contract.Name, description: contract.Description,
                validFrom: contract.ValidFrom, validTo: contract.ValidTo);


        private readonly IContractManagerContextService _contractManagerContext;
        private readonly DirectContracts.Services.Management.IContractManagementRepository _contractManagementRepository;
        private readonly DirectContracts.Services.Management.IAccommodationManagementRepository _accommodationManagementRepository;
    }
}