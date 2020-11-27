﻿using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Common.Models;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public interface IContractManagerContextService
    {
        Task<Result<Manager>> GetContractManager();

        string GetIdentityHash();

        Task<bool> DoesContractManagerExist();
    }
}