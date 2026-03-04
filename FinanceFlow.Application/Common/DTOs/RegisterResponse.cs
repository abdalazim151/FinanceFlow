using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Common.DTOs
{
    public class RegisterResponse
    {
        public RegisterResponse(string msg, bool success)
        {
            IsSuccess = success;
            Message = msg;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
