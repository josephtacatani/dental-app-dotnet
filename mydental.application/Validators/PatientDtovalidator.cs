using FluentValidation;
using mydental.application.DTO.PatientDTO;
using mydental.domain.Helpers;

namespace mydental.application.Validators
{
    public class PatientDtoValidator : AbstractValidator<PatientDto>
    {
        public PatientDtoValidator()
        {
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(p => p.ContactNumber)
                .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid contact number format");

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

    public class PatientUpdatePayloadValidator : AbstractValidator<PatientUpdatePayloadDto>
    {
        public PatientUpdatePayloadValidator()
        {
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("Full name is required");

            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Address is required");

            RuleFor(p => p.ContactNumber)
                .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid contact number format");
        }
    }
}
