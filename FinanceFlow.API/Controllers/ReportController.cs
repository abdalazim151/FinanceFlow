using FinanceFlow.Infrastructure.Identity;
using FinanceFlow.Infrastructure.Serivces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FinanceFlow.Domain.Enums;

namespace FinanceFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService service;
        private readonly UserManager<ApplicationUser> userManager;

        public ReportController(
            UserManager<ApplicationUser> userManager ,
             ReportService service)
        {
            this.service = service;
            this.userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<string>> Get(string Type)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return BadRequest("NO ID");
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("NoUser");
            if (Enum.TryParse<TransactionType>(Type, true, out var val))
            {
                if (Type.ToLower() == "Feed") return Forbid("Not allowed");
                await service.Generate(Type, user.Email);
                return Ok("Your Request has been submited we will send it succesfuly");
            }
            else return BadRequest("NO such Type");           
        }
        [HttpGet("AdminReport")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<string>> GetReport(string Type)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return BadRequest("NO ID");
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("NoUser");
            service.Generate(Type, user.Email);
            return Ok("Your Request has been submited we will send it succesfuly");
        }

    }
}
