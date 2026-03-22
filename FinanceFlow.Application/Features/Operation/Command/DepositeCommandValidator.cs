using FluentValidation;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public class DepositeCommandValidator : AbstractValidator<DepositeCommand>
    {
        public DepositeCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
            RuleFor(x => x.AtmId)
                .GreaterThan(0).WithMessage("AtmId must be greater than zero.");
        }
    }
}

