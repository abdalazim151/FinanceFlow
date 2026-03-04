using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Common.DTOs
{
    public class LoginResponse
    {

        public LoginResponse(string v, bool isSuccess)
        {
            Token = v;
            IsSuccess = isSuccess;
        }
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
    }
}
