using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Features.Authentication.Commands;
using FinanceFlow.Infrastructure.Serivces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IMediator mediator;
        private EmailService emailService;
        private readonly ReportService reportService;

        public AccountController(IMediator mediator
            , EmailService service
            ,ReportService reportService)
        {
            this.mediator = mediator;
            this.emailService = service;
            this.reportService = reportService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterCommand request)
        {
            var res = await mediator.Send(request);
            if (res.IsSuccess)
                return Ok(res);
            return BadRequest(res);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginCommand req)
        {
            var res = await mediator.Send(req);
            if (res.IsSuccess)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> TestEmail()
        {
            try
            {

                emailService.Send("abdalazim.ahmed20@gmail.com", "test", "test,test");
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("TestReport")]
        public async Task<ActionResult> testReport()
        {
            await reportService.Generate("Feed","abdalazim.ahmed20@gmail.com");
            return Ok(true);

        }
    }
}
