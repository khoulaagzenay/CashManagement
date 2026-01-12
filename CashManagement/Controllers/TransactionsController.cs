using CashManagement.Models.DTOs;
using CashManagement.Models.DTOs.Roles;
using CashManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRolesDto.User)]
    public class TransactionsController : ControllerBase
    {
            private readonly ITransactionService _transactionService;

            public TransactionsController(ITransactionService transactionService)
            {
                _transactionService = transactionService;
            }
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetById(Guid id)
            {
                var transaction = await _transactionService.GetByIdAsync(id);
                if (transaction == null)
                    return NotFound(new { message = "Transaction not found." });

                return Ok(transaction);
            }

            [HttpGet("account/{accountId:guid}")]
            public async Task<IActionResult> GetByAccount(Guid accountId)
            {
                var list = await _transactionService.GetByAccountAsync(accountId);
                return Ok(list);
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                try
                {
                    var created = await _transactionService.CreateAsync(dto);
                    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }

            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                try
                {
                    await _transactionService.DeleteAsync(id);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
    
    }
}
