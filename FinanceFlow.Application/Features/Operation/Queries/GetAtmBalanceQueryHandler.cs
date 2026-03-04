using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Queries
{
    public record GetAtmBalanceQuery(int AtmId) : IRequest<int>;

    public class GetAtmBalanceQueryHandler : IRequestHandler<GetAtmBalanceQuery, int>
    {
        private readonly IAtmService atmService;

        public GetAtmBalanceQueryHandler(IAtmService atmService)
        {
            this.atmService = atmService;
        }

        public async Task<int> Handle(GetAtmBalanceQuery request, CancellationToken cancellationToken)
        {
            return await atmService.GetAtmBalanceAsync(request.AtmId);
        }
    }
}

