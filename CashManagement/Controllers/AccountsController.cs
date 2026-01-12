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
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
            public AccountsController(IAccountService accountService)
            {
                _accountService = accountService;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var accounts = await _accountService.GetAllAsync();
                return Ok(accounts);
            }
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetById(Guid id)
            {
                var account = await _accountService.GetByIdAsync(id);
                if (account == null)
                    return NotFound(new { message = "Account not found." });

                return Ok(account);
            }
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] AccountDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _accountService.CreateAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }

            [HttpPut("{id:guid}")]
            public async Task<IActionResult> Update(Guid id, [FromBody] AccountDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var exists = await _accountService.AccountExistsAsync(id);
                if (!exists)
                    return NotFound(new { message = "Account not found." });

                await _accountService.UpdateAsync(id, dto);

                return NoContent();
            }

            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                var exists = await _accountService.AccountExistsAsync(id);
                if (!exists)
                    return NotFound(new { message = "Account not found." });

                try
                {
                    await _accountService.DeleteAsync(id);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }

                return NoContent();
            }
            [HttpGet("{id:guid}/balance")]
            public async Task<IActionResult> GetBalance(Guid id)
            {
                var exists = await _accountService.AccountExistsAsync(id);
                if (!exists)
                    return NotFound(new { message = "Account not found." });

                var balance = await _accountService.GetBalanceAsync(id);
                return Ok(new { accountId = id, balance });
      }
    }
 }



