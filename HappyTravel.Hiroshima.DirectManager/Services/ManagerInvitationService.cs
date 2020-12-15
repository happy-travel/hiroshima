using CSharpFunctionalExtensions;
using System;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class ManagerInvitationService : IManagerInvitationService
    {
        public Task Accept(string invitationCode)
        {
            throw new NotImplementedException();
        }


        public Task<Result<Models.Requests.ManagerInvitation>> GetPendingInvitation(string invitationCode)
        {
            throw new NotImplementedException();
        }
    }
}
