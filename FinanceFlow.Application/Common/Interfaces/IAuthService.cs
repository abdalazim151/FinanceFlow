using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Features.Authentication.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterCommand command);
        Task<LoginResponse> LoginAsync(LoginCommand command);
    }
}
