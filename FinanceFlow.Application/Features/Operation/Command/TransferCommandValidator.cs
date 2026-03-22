using FluentValidation;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public class TransferCommandValidator : AbstractValidator<TransferCommand>
    {
        public TransferCommandValidator()
        {

            RuleFor(x => x.ToAccountId)
                .NotEmpty().WithMessage("ToAccountId is required.")
                .NotEqual(x => x.FromAccountId).WithMessage("Source and destination accounts must be different.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

          
        }
    }
}

