using FluentValidation;
using mydental.application.DTO.AuthDTO;
using mydental.domain.Helpers;

namespace mydental.application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.");

            RuleFor(p => p.BirthDate)
                .Must(IsValidDate).WithMessage("Invalid date format. Use yyyy-MM-dd.")
                .Must(birthDate => DateHelper.ParseDate(birthDate) < DateTime.Today)
                .WithMessage("Birthdate must be in the past");
        }

        private bool IsValidDate(string birthDate)
        {
            return DateHelper.ParseDate(birthDate).HasValue;
        }
    }
}
