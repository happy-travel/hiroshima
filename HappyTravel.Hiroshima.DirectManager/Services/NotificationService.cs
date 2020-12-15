using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Mailing;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.DirectManager.Models.Requests;
using HappyTravel.MailSender;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HappyTravel.Hiroshima.DirectManager.Services
{
    public class NotificationService : INotificationService
    {
        public NotificationService(IMailSender mailSender, ICompanyService companyService, IOptions<NotificationServiceOptions> options)
        {
            _mailSender = mailSender;
            _companyService = companyService;
            _options = options;
        }


        public async Task<Result> SendRegistrationConfirmation(string emailMasterManager, ManagerInfo managerInfo, string serviceSupplierName)
        {
            var position = managerInfo.Position;
            if (string.IsNullOrWhiteSpace(position))
                position = "a new employee";

            var companyInfo = await _companyService.Get();

            return await _mailSender.Send<RegistrationDataForMaster>(_options.Value.RegularManagerMailTemplateId, emailMasterManager, new RegistrationDataForMaster
            {
                ManagerName = $"{managerInfo.FirstName} {managerInfo.LastName}",
                Position = position,
                Title = managerInfo.Title,
                ServiceSupplierName = serviceSupplierName,
                CompanyInfo = companyInfo.IsFailure
                    ? new CompanyInfo()
                    : companyInfo.Value
            });
        }


        private readonly IMailSender _mailSender;
        private readonly ICompanyService _companyService;
        private readonly IOptions<NotificationServiceOptions> _options;
    }
}
