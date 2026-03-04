using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Queries
{
    public record GetAllAtmQuery() : IRequest<IReadOnlyList<AtmDto>>;

    public class GetAllAtmQueryHandler : IRequestHandler<GetAllAtmQuery, IReadOnlyList<AtmDto>>
    {
        private readonly IAtmService atmService;

        public GetAllAtmQueryHandler(IAtmService atmService)
        {
            this.atmService = atmService;
        }

        public async Task<IReadOnlyList<AtmDto>> Handle(GetAllAtmQuery request, CancellationToken cancellationToken)
        {
            return await atmService.GetAllAtmsAsync();
        }
    }
}

