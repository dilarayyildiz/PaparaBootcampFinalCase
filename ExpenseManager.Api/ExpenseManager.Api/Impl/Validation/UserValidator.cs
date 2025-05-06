using ExpenseManager.Api.Impl.Cqrs;
using ExpenseManager.Schema;
using FluentValidation;

namespace ExpenseManager.Api.Impl.Validation;

public class UserValidator : AbstractValidator<UserRequest>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
        RuleFor(x => x.Role).NotEmpty().Must(r => r == "Admin" || r == "Employee")
            .WithMessage("Role must be either 'Admin' or 'Employee'.");
        RuleFor(x => x.IBAN).NotEmpty();
    }
    
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.User).SetValidator(new UserValidator());
        }
    }
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.User).SetValidator(new UserValidator());
        }
    }
}