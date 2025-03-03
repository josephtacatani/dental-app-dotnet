using FluentValidation;
using mydental.application.DTO;
using mydental.application.DTO.ServiceListDTO;

namespace mydental.application.Validators
{
    public class ServiceListDtoValidator : AbstractValidator<ServiceListDto>
    {
        public ServiceListDtoValidator()
        {
            RuleFor(x => x.ServiceName)
                .NotEmpty().WithMessage("Service name is required.")
                .MaximumLength(255).WithMessage("Service name must not exceed 255 characters.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(10).WithMessage("Content must be at least 10 characters long.");

            RuleFor(x => x.Photo)
                .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters.")
                .Matches(@"^(http|https)://.*").When(x => !string.IsNullOrEmpty(x.Photo))
                .WithMessage("Photo must be a valid URL.");
        }
    }
}
