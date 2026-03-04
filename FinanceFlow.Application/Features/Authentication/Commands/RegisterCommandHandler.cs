using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;






namespace FinanceFlow.Application.Features.Authentication.Commands
{
    public record RegisterCommand(
        string FullName,
    string Email,
    string Password,
    string ConfirmPassword
        ) : IRequest<RegisterResponse>;
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand,RegisterResponse>
    {
        private readonly IAuthService authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var res = await authService.RegisterAsync(request);
            return res;

        }
    }
}
