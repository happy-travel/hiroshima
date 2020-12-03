using FluentValidation;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    class ManagerPermissionsRequestValidator : AbstractValidator<ManagerPermissions>
    {
        public ManagerPermissionsRequestValidator()
        {
            RuleFor(managerPermissions => managerPermissions.Permissions).NotEmpty();
        }
    }
}
