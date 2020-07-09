using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class UserContextService : IUserContextService
    {
        public UserContextService(DirectContractsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public async Task<Result<User>> GetUser()
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id  == int.MaxValue);
            
            return user.Equals(default) 
                ? Result.Failure<User>("Failed to retrieve user") 
                : Result.Ok(user);
        }

        
        private readonly DirectContractsDbContext _dbContext;
    }
}