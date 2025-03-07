using System.ComponentModel.DataAnnotations;
using ECommerce.Core.DTO;
using FluentValidation;

namespace ECommerce.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        
        RuleFor(x => x.PersonName)
            .NotEmpty().WithMessage("Person Name is required")
            .MaximumLength(50).WithMessage("Person Name must have up to 50 characters");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Male, Female or Other are the only valid options");
    }
}