using FluentValidation;

namespace ElementLogiq.eGlobalShop.Customers.Application.Create;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("A valid email address is required")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\+?[0-9\s\-\(\)]+$")
            .WithMessage("Phone number is not in a valid format");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of birth is required")
            .Must(BeAValidAge)
            .WithMessage("Customer must be at least 18 years old and not born in the future");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(200)
            .WithMessage("Address cannot exceed 200 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City cannot exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State is required")
            .MaximumLength(50)
            .WithMessage("State cannot exceed 50 characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code is required")
            .Matches(@"^\d{5}(-\d{4})?$")
            .WithMessage("Zip code is not in a valid format (e.g. 12345 or 12345-6789)");
    }

    private static bool BeAValidAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dateOfBirth.Year;

        // Adjust age if birthday hasn't occurred yet this year
        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        // Check if the person is at least 18 and not born in the future
        return age >= 18 && dateOfBirth <= today;
    }
}
