using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class ManagerInfoWithCodeValidator : AbstractValidator<ManagerInfoWithCode>
    {
        public ManagerInfoWithCodeValidator()
        {
            RuleFor(managerInfo => managerInfo.FirstName).NotEmpty();
            RuleFor(managerInfo => managerInfo.LastName).NotEmpty();
            RuleFor(managerInfo => managerInfo.Title).NotEmpty();
            RuleFor(managerInfo => managerInfo.InvitationCode).NotEmpty();
        }
    }
}
