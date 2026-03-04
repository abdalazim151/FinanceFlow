using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Features.Authentication.Commands;
namespace FinanceFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterCommand request)
        {
            var res = await mediator.Send(request);
            if (res.IsSuccess)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginCommand req)
        {
            var res = await mediator.Send(req);
            if (res.IsSuccess)
                return Ok(res);
            return BadRequest(res);
        }
    }
}
