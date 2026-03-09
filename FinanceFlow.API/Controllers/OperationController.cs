using System.Security.Claims;
using FinanceFlow.Application.Common.DTOs;
using FinanceFlow.Application.Features.Operation.Command;
using FinanceFlow.Application.Features.Operation.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperationController : ControllerBase
    {
        private readonly IMediator mediator;

        public OperationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposite([FromBody] DepositeCommand command)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var enrichedCommand = command with { AccountId = userId };

            var result = await mediator.Send(enrichedCommand);
            if (!result)
            {
                return BadRequest("Deposit failed.");
            }

            return Ok(true);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawCommand command)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var enrichedCommand = command with { AccountId = userId };

            var result = await mediator.Send(enrichedCommand);
            if (!result)
            {
                return BadRequest("Withdraw failed.");
            }

            return Ok(true);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferCommand command)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var enrichedCommand = command with { FromAccountId = userId };

            var result = await mediator.Send(enrichedCommand);
            if (!result)
            {
                return BadRequest("Transfer failed.");
            }

            return Ok(true);
        }

        [HttpPost("bank-feed")]
        public async Task<IActionResult> BankFeed([FromBody] BankFeedCommand command)
        {
            var result = await mediator.Send(command);
            if (!result)
            {
                return BadRequest("Bank feed failed.");
            }

            return Ok(true);
        }

        [HttpGet("users/me/balance")]
        public async Task<ActionResult<decimal>> GetUserBalance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var balance = await mediator.Send(new GetUserBalanceQuery(userId));
            return Ok(balance);
        }

        [HttpGet("atms/{atmId}/balance")]
        public async Task<ActionResult<int>> GetAtmBalance(int atmId)
        {
            var balance = await mediator.Send(new GetAtmBalanceQuery(atmId));
            return Ok(balance);
        }

        [HttpGet("atms")]
        public async Task<ActionResult<IReadOnlyList<AtmDto>>> GetAllAtm()
        {
            var result = await mediator.Send(new GetAllAtmQuery());
            return Ok(result);
        }

        [HttpPost("atms")]
        public async Task<ActionResult<AtmDto>> CreateAtm([FromBody] CreateAtmCommand command)
        {
            var createdAtm = await mediator.Send(command);

            return CreatedAtAction(
                nameof(GetAtmBalance),
                new { atmId = createdAtm.Id },
                createdAtm
            );
        }
    }
}