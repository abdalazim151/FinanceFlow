using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Features.Authentication.Commands
{
    public record LoginCommand(
            string Email,
            string password,
            bool rememberMe
        ):IRequest<LoginResponse>;
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IAuthService authService;

        public LoginCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var res =await authService.LoginAsync(request);
            return res;
        }
    }
}
