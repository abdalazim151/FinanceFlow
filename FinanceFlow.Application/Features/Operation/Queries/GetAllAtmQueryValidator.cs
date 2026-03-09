using FluentValidation;

namespace FinanceFlow.Application.Features.Operation.Queries
{
    public class GetAllAtmQueryValidator : AbstractValidator<GetAllAtmQuery>
    {
        public GetAllAtmQueryValidator()
        {
            // No fields currently, but this keeps the pattern consistent
        }
    }
}

