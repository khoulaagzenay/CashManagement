using CashManagement.Models.DTOs.Roles;
using CashManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =UserRolesDto.Admin)]
    public class ForecastController : ControllerBase
    {
        private readonly IForecastService _forecastService;

        public ForecastController(IForecastService forecastService)
        {
            _forecastService = forecastService;
        }
        [HttpGet("{accountId:guid}/{date}")]
        public async Task<IActionResult> GetDailyBalance(Guid accountId, DateTime date)
        {
            var balance = await _forecastService.GetDailyBalanceAsync(accountId, date);

            if (balance == null)
                return NotFound(new { message = "No daily balance found for this date." });

            return Ok(balance);
        }
        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> Generate(
            Guid accountId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end)
        {
            if (start == default || end == default)
                return BadRequest(new { message = "'start' & 'end' parameters are required." });

            try
            {
                var result = await _forecastService.GenerateDailyBalancesAsync(accountId, start, end);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}



