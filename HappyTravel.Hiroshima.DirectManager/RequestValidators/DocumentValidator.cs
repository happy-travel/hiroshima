using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class DocumentValidator : AbstractValidator<Models.Requests.Document>
    {
        public DocumentValidator()
        {
            RuleFor(document => document.Name).NotEmpty();
            RuleFor(document => document.MimeType).NotEmpty();
            RuleFor(document => document.ContractId).NotEmpty();
            RuleFor(document => document.FileContent).NotEmpty();
        }

    }
}
