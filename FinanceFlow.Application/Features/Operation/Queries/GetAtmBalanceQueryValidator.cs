using FluentValidation;

namespace FinanceFlow.Application.Features.Operation.Queries
{
    public class GetAtmBalanceQueryValidator : AbstractValidator<GetAtmBalanceQuery>
    {
        public GetAtmBalanceQueryValidator()
        {
            RuleFor(x => x.AtmId)
                .GreaterThan(0).WithMessage("AtmId must be greater than zero.");
        }
    }
}

