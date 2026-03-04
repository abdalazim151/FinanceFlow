using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Queries
{
    public record GetUserBalanceQuery(string AccountId) : IRequest<decimal>;

    public class GetUserBalanceQueryHandler : IRequestHandler<GetUserBalanceQuery, decimal>
    {
        private readonly IAtmService atmService;

        public GetUserBalanceQueryHandler(IAtmService atmService)
        {
            this.atmService = atmService;
        }

        public async Task<decimal> Handle(GetUserBalanceQuery request, CancellationToken cancellationToken)
        {
            return await atmService.GetUserBalanceAsync(request.AccountId);
        }
    }
}

