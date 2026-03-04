using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public record TransferCommand(
        string FromAccountId,
        string ToAccountId,
        int Amount,
        int AtmId,
        string Description
    ) : IRequest<bool>;

    public class TransferCommandHandler : IRequestHandler<TransferCommand, bool>
    {
        private readonly ITransferService transferService;

        public TransferCommandHandler(ITransferService transferService)
        {
            this.transferService = transferService;
        }

        public async Task<bool> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            return await transferService.TransferAsync(
                request.FromAccountId,
                request.ToAccountId,
                request.Amount,
                request.AtmId,
                request.Description
            );
        }
    }
}

