using FluentValidation;

namespace FinanceFlow.Application.Features.Operation.Queries
{
    public class GetUserBalanceQueryValidator : AbstractValidator<GetUserBalanceQuery>
    {
        public GetUserBalanceQueryValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("AccountId is required.");
        }
    }
}

