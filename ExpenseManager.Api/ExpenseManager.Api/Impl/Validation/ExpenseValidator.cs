using ExpenseManager.Api.Request;
using ExpenseManager.Schema;
using FluentValidation;

namespace ExpenseManager.Api.Impl.Validation;

public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseRequestValidator()
    {
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.PaymentMethod).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.PaymentLocation).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.ReceiptFile).NotNull().WithMessage("ReceiptFile is required.");
    }
}

public class ExpenseRequestValidator : AbstractValidator<ExpenseRequest>
{
    public ExpenseRequestValidator()
    {
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.PaymentMethod).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.PaymentLocation).NotEmpty().MinimumLength(2).MaximumLength(50);
    }
}

public class RejectExpenseRequestValidator : AbstractValidator<RejectExpenseRequest>
{
    public RejectExpenseRequestValidator()
    {
        RuleFor(x => x.RejectionReason).NotEmpty().MinimumLength(2).MaximumLength(50);
    }
}

