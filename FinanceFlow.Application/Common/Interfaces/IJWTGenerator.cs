using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Common.Interfaces
{
    public interface IJWTGenerator
    {
        Task<LoginResponse> CreateTokenAsync(User user);


    }
}
