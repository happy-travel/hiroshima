using FluentValidation;

namespace HappyTravel.Hiroshima.DirectManager.RequestValidators
{
    public class RoomImageValidator : AbstractValidator<Models.Requests.RoomImage>
    {
        public RoomImageValidator()
        {
            RuleFor(image => image).NotNull();
            RuleFor(image => image.AccommodationId).NotEmpty();
            RuleFor(image => image.RoomId).NotEmpty();
            RuleFor(image => image.UploadedFile).NotEmpty();
        }
    }
}
