using FluentValidation;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
    {
        public WithdrawCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
            RuleFor(x => x.AtmId)
                .GreaterThan(0).WithMessage("AtmId must be greater than zero.");
        }
    }
}

