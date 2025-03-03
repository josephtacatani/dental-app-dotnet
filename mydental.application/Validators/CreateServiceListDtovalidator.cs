using FluentValidation;
using mydental.application.DTO.ServiceListDTO;

namespace mydental.application.Validators
{
    public class CreateServiceListDtoValidator : AbstractValidator<CreateServiceListDto>
    {
        public CreateServiceListDtoValidator()
        {
            RuleFor(x => x.ServiceName)
                .NotEmpty().WithMessage("Service name is required.")
                .MaximumLength(255).WithMessage("Service name must not exceed 255 characters.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");

            RuleFor(x => x.Photo)
                .NotEmpty().WithMessage("Photo URL is required.")
                .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters.");
        }
    }
}
