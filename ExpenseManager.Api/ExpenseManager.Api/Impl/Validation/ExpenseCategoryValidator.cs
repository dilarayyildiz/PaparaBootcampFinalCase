using ExpenseManager.Schema;
using FluentValidation;

namespace ExpenseManager.Api.Impl.Validation;

public class ExpenseCategoryRequestValidator : AbstractValidator<ExpenseCategoryRequest>
{
    public ExpenseCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}