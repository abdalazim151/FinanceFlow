using FluentValidation;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public class CreateAtmCommandValidator : AbstractValidator<CreateAtmCommand>
    {
        public CreateAtmCommandValidator()
        {
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");
        }
    }
}

