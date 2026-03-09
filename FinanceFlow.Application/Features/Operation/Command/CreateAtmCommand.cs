using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public record CreateAtmCommand(string Location) : IRequest<AtmDto>;

    public class CreateAtmCommandHandler : IRequestHandler<CreateAtmCommand, AtmDto>
    {
        private readonly IAtmService atmService;

        public CreateAtmCommandHandler(IAtmService atmService)
        {
            this.atmService = atmService;
        }

        public async Task<AtmDto> Handle(CreateAtmCommand request, CancellationToken cancellationToken)
        {
            return await atmService.CreateAtmAsync(request.Location);
        }
    }
}

