using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public record BankFeedCommand(
        int AtmId,
        int Amount,
        string Description
    ) : IRequest<bool>;

    public class BankFeedCommandHandler : IRequestHandler<BankFeedCommand, bool>
    {
        private readonly IAtmService atmService;

        public BankFeedCommandHandler(IAtmService atmService)
        {
            this.atmService = atmService;
        }

        public async Task<bool> Handle(BankFeedCommand request, CancellationToken cancellationToken)
        {
            return await atmService.BankFeedAsync(
                request.AtmId,
                request.Amount,
                request.Description
            );
        }
    }
}

